using System.Reflection;

namespace Ct1300_Evidence.Pages
{
	public partial class EvidenceZisku
	{
		#region Vlastnosti
		/// <summary>
		/// Seznam položek evidence zisku.
		/// </summary>
		public List<Models.Polozka> Polozky { get; set; } = new List<Models.Polozka>();

		public Models.Polozka Polozka { get; set; } = new Models.Polozka();

		#endregion
	}
}
