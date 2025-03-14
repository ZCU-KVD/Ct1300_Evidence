﻿using Microsoft.JSInterop;
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

		/// <summary>
		/// Textový výpis výsledků pro různé akce, například počet záznamů nebo statistiky.
		/// </summary>
		public string? Vypis { get; private set; }
		/// <summary>
		/// Počet všech záznamů v seznamu položek.
		/// </summary>
		public int PocetZaznamu => Polozky.Count;
		/// <summary>
		/// Indikuje, zda seznam položek není prázdný.
		/// </summary>
		public bool IsPolozky => Polozky.Count > 0;

		/// <summary>
		/// Indikuje, zda je seznam položek prázdný.
		/// </summary>
		public bool IsNotPolozky => !IsPolozky;
		
		/// <summary>
		/// Určuje, zda se má zobrazit filtr dat a výsledky filtrování.
		/// </summary>
		public bool ZobrazenFiltrDat { get; set; } = false;
		/// <summary>
		/// Určuje typ filtru pro zisk: &lt;, = nebo &gt;.
		/// </summary>
		public string SelectedFilter { get; set; } = "=";
		/// <summary>
		/// Číselná hodnota, podle které se filtruje zisk položek.
		/// </summary>
		public double FiltrHodnota { get; set; }

		/// <summary>
		/// Textový filtr pro porovnávání s obsahem popisu položek.
		/// </summary>
		public string FiltrPopis { get; set; } = "";
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


		/// <summary>
		/// Zobrazí všechny položky v textovém řetězci.
		/// </summary>
		public void ZobrazZaznamy()
		{
			Vypis = "Jednotlivé záznamy: <br>" + string.Join("<br>", Polozky);
		}

		/// <summary>
		/// Zobrazí počet všech zaznamenaných položek.
		/// </summary>
		public void ZobrazPocetZaznamu()
		{
			Vypis = $"Počet záznamů: {PocetZaznamu}";
		}

		/// <summary>
		/// Zobrazí statistiky (minimum, maximum a průměr) zisků.
		/// </summary>
		public void Statistiky()
		{
			string vypis = "";
			vypis += "Minimum: " + Minimum().ToString("C2") + "<br>";
			vypis += "Maximum: " + Maximum().ToString("C2") + "<br>";
			vypis += "Průměr: " + Prumer().ToString("C2");
			Vypis = vypis;
		}

		/// <summary>
		/// Vrátí minimální zisk ze všech položek.
		/// </summary>
		/// <returns>Nejmenší hodnota zisku, nebo NaN pokud je seznam prázdný.</returns>
		public double Minimum()
		{
			if (Polozky.Count == 0)
			{
				return double.NaN;
			}
			return Polozky.Min(x => x.Zisk);
		}

		/// <summary>
		/// Vrátí maximální zisk ze všech položek.
		/// </summary>
		/// <returns>Největší hodnota zisku, nebo NaN pokud je seznam prázdný.</returns>
		public double Maximum()
		{
			if (Polozky.Count == 0)
			{
				return double.NaN;
			}
			return Polozky.Max(x => x.Zisk);
		}

		/// <summary>
		/// Vrátí průměrný zisk ze všech položek.
		/// </summary>
		/// <returns>Průměrná hodnota zisku, nebo NaN pokud je seznam prázdný.</returns>
		private double Prumer()
		{
			if (Polozky.Count == 0)
			{
				return double.NaN;
			}
			return Polozky.Average(x => x.Zisk);
		}

		/// <summary>
		/// Seřadí položky vzestupně podle jejich data.
		/// </summary>
		public void Setridit()
		{
			Polozky.Sort((x, y) => x.Datum.CompareTo(y.Datum));
		}

		/// <summary>
		/// Seřadí položky sestupně podle jejich data.
		/// </summary>
		public void SetriditSestupne()
		{
			Polozky.Sort((x, y) => y.Datum.CompareTo(x.Datum));
		}

		/// <summary>
		/// Vrací CSS třídu pro zvýraznění vybrané položky v tabulce, je-li právě editována.
		/// </summary>
		/// <param name="polozka">Položka, pro kterou se vyhodnocuje CSS třída.</param>
		/// <returns>Název CSS třídy, pokud je položka editována; jinak prázdný řetězec.</returns>
		private string GetCssClassForTR(Models.Polozka polozka)
		{
			return polozka == Polozka ? "table-primary" : "";
		}

		/// <summary>
		/// Filtrované položky podle typu filtru zisku a volitelného filtru popisu položky.
		/// </summary>
		public void FiltrujPolozky()
		{

		}
		#endregion
	}
}
