using Microsoft.AspNetCore.Components;

namespace Evidence.Pages
{
	public partial class EvidenceZisku
	{
		[Inject] private Services.EvidenceService EvidenceService { get; set; } = default!;

		//private Services.EvidenceService EvidenceService { get; set; } = new Services.EvidenceService();
		#region Životní cyklus komponenty

		protected override Task OnInitializedAsync()
		{
			if(EvidenceService.TransakceSeznam.Count == 0)
				EvidenceService.VygenerovatNahodnaData(1);

			return base.OnInitializedAsync();
		}
		#endregion

		#region Stav komponnety
		private Models.Transakce formularTransakce = new Models.Transakce();
		#endregion

		#region Formulář CRUD
		private void UlozitTransakci()
		{
			EvidenceService.PridatTransakci(formularTransakce);

			formularTransakce = new Models.Transakce();
		}
		#endregion

	}
}
