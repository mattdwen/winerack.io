using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace my.winerack.io.Models {

	public class Wine {

		#region Properties

		public int ID { get; set; }

		public string Name { get; set; }

		[Required]
		public string Varietal { get; set; }

		[DisplayFormat(NullDisplayText = "NV")]
		public int? Vintage { get; set; }

		[Display(Name = "Region")]
		[ForeignKey("Region")]
		public int RegionID { get; set; }

		[Display(Name = "Vineyard")]
		[ForeignKey("Vineyard")]
		public int VineyardID { get; set; }

		[NotMapped]
		public string Description {
			get {
				return Vineyard.Name + " " + Varietal + " " + Vintage.ToString();
			}
		}

		#endregion Properties

		#region Relationships

		public virtual Region Region { get; set; }

		public virtual Vineyard Vineyard { get; set; }

		#endregion Relationships
	}
}