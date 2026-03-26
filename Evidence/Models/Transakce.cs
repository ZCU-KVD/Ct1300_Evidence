using System.ComponentModel.DataAnnotations;

namespace Evidence.Models
{
	public class Transakce
	{
		private string popis = "";
		private decimal vynosy;
		private decimal naklady;
		public Transakce() { }
		public Transakce(DateOnly datum, decimal vynosy, decimal naklady, string popis) 
		{ 
			Datum = datum;
			Vynosy = vynosy;
			Naklady = naklady;
			Popis = popis;
		}
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required(ErrorMessage = "Datum je povinný")]
		public DateOnly Datum { get; set; } = DateOnly.FromDateTime(DateTime.Now);

		[Required(ErrorMessage = "Popis je povinný")]
		public string Popis
		{
			get => popis;
			set
			{
				if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Popis nesmí být prázdný nebo obsahovat pouze mezery.");
				popis = value;
			}
		}
		[Range(0, double.MaxValue, ErrorMessage = "Výnosy musí být nezáporné")]
		public decimal Vynosy
		{
			get => vynosy;
			set
			{
				if (value < 0) throw new ArgumentException("Výnosy musí být nezáporné.");
				vynosy = value;
			}
		}

		[Range(0, double.MaxValue, ErrorMessage = "Náklady musí být nezáporné")]
		public decimal Naklady
		{
			get => naklady;
			set
			{
				if (value < 0) throw new ArgumentException("Náklady musí být nezáporné.");
				naklady = value;
			}
		}

		public decimal Zisk => Vynosy - Naklady;
	}
}
