namespace Ct1300_Evidence.Models
{
	public class Polozka
	{
		public Polozka()
		{
			Datum = DateOnly.FromDateTime(DateTime.Now);
		}

		public DateOnly Datum { get; set; }
		public double Naklady { get; set; }
		public double Vynosy { get; set; }
		public string Popis { get; set; } = "";
		public double Zisk => Vynosy - Naklady;
	}
}
