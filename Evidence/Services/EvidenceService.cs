using Evidence.Models;

namespace Evidence.Services
{
	public class EvidenceService
	{
		public List<Transakce> TransakceSeznam { get; set; } = new List<Transakce>();

		public void PridatTransakci(Transakce novaTransakce)
		{
			TransakceSeznam.Add(novaTransakce);
		}

		public void VygenerovatNahodnaData(int pocet) 
		{ 
			TransakceSeznam.Clear();
			var rnd = new Random();
			string[] popisy = { "Prodej zboží", "Konzultace", "Služby", "Oprava zařízení", "Pronájem" };
			for (int i = 0; i < pocet; i++)
			{
				var transakce = new Transakce(
					 datum: DateOnly.FromDateTime(DateTime.Now.AddDays(-rnd.Next(0, 365))),
					 vynosy: rnd.Next(1000, 50000),
					 naklady: rnd.Next(500, 25000),
					 popis: popisy[rnd.Next(popisy.Length)]
					);
				
				TransakceSeznam.Add(transakce);
			}

		}
	}
}
