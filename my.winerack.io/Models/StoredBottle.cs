using System.ComponentModel.DataAnnotations.Schema;

namespace winerack.Models {

	public class StoredBottle {

		#region Declarations

		public int ID { get; set; }

		[ForeignKey("Bottle")]
		public int BottleID { get; set; }

		public string Location { get; set; }

		#endregion Declarations

		#region Relationships

		public virtual Bottle Bottle { get; set; }

		#endregion Relationships
	}
}