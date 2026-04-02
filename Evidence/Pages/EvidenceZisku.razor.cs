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
		#endregion

	}
}
