using Microsoft.JSInterop;
using System.Reflection;

namespace Ct1300_Evidence.Pages
{

	/// <summary>
	/// Třída EvidenceZisku představuje komponentu pro správu evidence zisku.
	/// </summary>
	public partial class EvidenceZisku
	{
		#region Vlastnosti
		/// <summary>
		/// Seznam položek evidence zisku.
		/// </summary>
		public List<Models.Polozka> Polozky { get; set; } = new List<Models.Polozka>();

		/// <summary>
		/// Aktuální položka pro přidání nebo úpravu.
		/// </summary>
		public Models.Polozka Polozka { get; set; } = new Models.Polozka();

		/// <summary>
		/// Indikátor, zda je položka v režimu úpravy.
		/// </summary>
		public bool IsEditace { get; set; } = false;
		#endregion

		#region Metody
		/// <summary>
		/// Přidá novou položku do seznamu položek.
		/// </summary>
		private void PridatPolozku()
		{
			Polozky.Add(new Models.Polozka(Polozka.Datum, Polozka.Naklady, Polozka.Vynosy, Polozka.Popis));
		}

		/// <summary>
		/// Smaže zadanou položku ze seznamu položek po potvrzení uživatelem.
		/// </summary>
		/// <param name="polozka">Položka, která má být smazána.</param>
		private async Task SmazatPolozku(Models.Polozka polozka)
		{
			var zprava = $"Opravdu chcete smazat položku z {polozka.Datum} se ziskem {polozka.Zisk}?";
			bool smazat = await JavaScript.InvokeAsync<bool>("confirm", zprava);
			if (smazat)
				Polozky.Remove(polozka);
		}

		/// <summary>
		/// Upraví zadanou položku.
		/// </summary>
		/// <param name="polozka">Položka, která má být upravena.</param>
		private void UpravitPolozku(Models.Polozka polozka)
		{
			Polozka = polozka;
			IsEditace = true;
		}

		/// <summary>
		/// Ukončí režim úpravy a resetuje aktuální položku.
		/// </summary>
		private void UkoncitEditaci()
		{
			IsEditace = false;
			Polozka = new Models.Polozka();
		}
		#endregion
	}
}
