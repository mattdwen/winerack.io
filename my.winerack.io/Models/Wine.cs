using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace winerack.Models {

	public class Wine {

		#region Properties

		public int ID { get; set; }

		public string Name { get; set; }

		[Required]
		[ForeignKey("Varietal")]
		public int? VarietalID { get; set; }

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
				string description = "";
				if (!string.IsNullOrWhiteSpace(Name)) {
					description = Name + " ";
				}

				if (Vintage.HasValue) {
					description += "'" + Vintage.ToString().Substring(2) + " ";
				}

				description += Varietal.Name;

				return description;
			}
		}

		#endregion Properties

		#region Relationships

		public virtual Region Region { get; set; }

		public virtual Varietal Varietal { get; set; }

		public virtual Vineyard Vineyard { get; set; }

		#endregion Relationships
	}
}