using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Evidence.Pages
{
	public partial class EvidenceZisku
	{
		[Inject] private Services.EvidenceService EvidenceService { get; set; } = default!;
		[Inject] private IJSRuntime JS { get; set; } = default!;

		//private Services.EvidenceService EvidenceService { get; set; } = new Services.EvidenceService();
		#region Životní cyklus komponenty

		protected override Task OnInitializedAsync()
		{
			if(EvidenceService.TransakceSeznam.Count == 0)
				EvidenceService.VygenerovatNahodnaData(10);

			return base.OnInitializedAsync();
		}
		#endregion

		#region Stav komponnety
		private Models.Transakce formularTransakce = new Models.Transakce();
		private Models.Transakce? originalEditovaneTransakce = null;

		private bool JeEditace => originalEditovaneTransakce != null;
		private string FiltrPopis { get; set; } = "";
		private decimal? FiltrZiskHodnota { get; set; }
		private Models.OperatorZisku FiltrZiskOperator { get; set; } = Models.OperatorZisku.Rovno;

		private List<Models.Transakce> FiltrovaneTransakce => EvidenceService.FiltrovatTransakce(FiltrPopis, FiltrZiskHodnota, FiltrZiskOperator);

		#endregion

		#region Formulář CRUD
		private void UlozitTransakci()
		{
			if (!JeEditace)
			{
				EvidenceService.PridatTransakci(formularTransakce);
			}
			else
			{
				EvidenceService.AktualizovatTransakci(originalEditovaneTransakce!, formularTransakce);
				originalEditovaneTransakce = null;
			}

			formularTransakce = new Models.Transakce();
		}

		private void ZrusitEditaci() 
		{
			originalEditovaneTransakce = null;
			formularTransakce = new Models.Transakce();
		}

		private async Task SmazatTransakci(Models.Transakce mazanaTransakce)
		{
			bool potvrzeni = await JS.InvokeAsync<bool>("confirm", $"Opravdu chcete smazat transakci: {mazanaTransakce}?");

			if (potvrzeni)
			{
				EvidenceService.OdeberTransakci(mazanaTransakce);
			}
		}

		private void EditovatTransakci(Models.Transakce editovanaTransakce)
		{ 
			//formularTransakce.Datum = editovanaTransakce.Datum;
			//formularTransakce.Popis = editovanaTransakce.Popis;
			formularTransakce = editovanaTransakce.Klonovat();
			originalEditovaneTransakce = editovanaTransakce;
		}

		private async Task SmazatVse() 
		{
			bool potvrzeni = await JS.InvokeAsync<bool>("confirm", $"Opravdu chcete smazat vše?");

			if (potvrzeni)
			{
				EvidenceService.SmazatVse();
			}
		}

		private async Task UlozitData() 
		{
			var jsonData = System.Text.Json.JsonSerializer.Serialize(EvidenceService.TransakceSeznam);
			await JS.InvokeVoidAsync("localStorage.setItem", "evidenceZiskuData", jsonData);
			await JS.InvokeVoidAsync("alert", "Data byla uložena do localStorage.");
		}

		private async Task NacistData()
		{
			try
			{
				var json = await JS.InvokeAsync<string>("localStorage.getItem", "evidenceZiskuData");
				if (!string.IsNullOrEmpty(json))
				{
					var nacteneTransakce = System.Text.Json.JsonSerializer.Deserialize<List<Models.Transakce>>(json);
					
					if (nacteneTransakce != null)
					{
						EvidenceService.TransakceSeznam = nacteneTransakce;
						await JS.InvokeVoidAsync("alert", "Data byla načtena z localStorage.");
					}
				}
			}
			catch (Exception ex)
			{
				await JS.InvokeVoidAsync("alert", "Chyba při načítání dat: " + ex.Message);
			}
		}
		private void ZrusitFiltr()
		{ 
			FiltrPopis = "";
			FiltrZiskHodnota = null;
			FiltrZiskOperator = Models.OperatorZisku.Rovno;
		}
		#endregion

	}
}
