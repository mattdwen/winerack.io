using System.ComponentModel.DataAnnotations.Schema;

namespace winerack.Models {

	public class StoredBottle {

		#region Declarations

		public int ID { get; set; }

		[ForeignKey("Purchase")]
		public int PurchaseID { get; set; }

		public string Location { get; set; }

		#endregion Declarations

		#region Relationships

		public virtual Purchase Purchase { get; set; }

		#endregion Relationships
	}
}