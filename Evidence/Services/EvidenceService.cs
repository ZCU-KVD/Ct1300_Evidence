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

		public void AktualizovatTransakci(Transakce puvodni, Transakce noveHodnoty)
		{
			puvodni.Aktualizovat(noveHodnoty);
		}

		public void OdeberTransakci(Transakce mazanaTransakce)
		{
			TransakceSeznam.Remove(mazanaTransakce);
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

		public List<Transakce> FiltrovatTransakce(string popis, decimal? ziskHodnota, OperatorZisku ziskOperator)
		{
			var vysledek = TransakceSeznam.AsEnumerable();
			if (!string.IsNullOrEmpty(popis))
			{
				vysledek = vysledek.Where(t => t.Popis.Contains(popis, StringComparison.OrdinalIgnoreCase));
			}
			if (ziskHodnota.HasValue)
			{
				vysledek = ziskOperator switch
				{
					OperatorZisku.Rovno => vysledek.Where(t => t.Zisk == ziskHodnota),
					OperatorZisku.VetsiNez => vysledek.Where(t => t.Zisk > ziskHodnota),
					OperatorZisku.MensiNez => vysledek.Where(t => t.Zisk < ziskHodnota),
					_ => vysledek
				};
			}

			return vysledek.ToList();
		}

		public void SmazatVse()
		{
			TransakceSeznam.Clear();
		}
	}
}
