using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace winerack.Models {

	public class Bottle {

		#region Constructor

		public Bottle() {
		}

		#endregion Constructor

		#region Properties

		public int ID { get; set; }

		[Required]
		[Display(Name = "Wine")]
		[ForeignKey("Wine")]
		public int WineID { get; set; }

		[Required]
		[Display(Name = "Owner")]
		[ForeignKey("Owner")]
		public string OwnerID { get; set; }

		[Required]
		[Display(Name = "Added")]
		public DateTime CreatedOn { get; set; }

		public int? CellarMin { get; set; }

		public int? CellarMax { get; set; }

		[NotMapped]
		[Display(Name = "Purchased")]
		public int NumberOfBottles {
			get {
                if (Purchases == null) return 0;
				return Purchases.Sum(p => p.StoredBottles.Count);
			}
		}

		[NotMapped]
		[Display(Name = "Opened")]
		public int NumberDrunk {
			get {
                if (Purchases == null) return 0;
				return Purchases.SelectMany(p => p.StoredBottles).Where(s => s.Opening != null).Count();
			}
		}

		[NotMapped]
		[Display(Name = "Remaining")]
		public int NumberRemaining {
			get {
                if (Purchases == null) return 0;
				return NumberOfBottles - NumberDrunk;
			}
		}

		#endregion Properties

		#region Relationships

		public virtual Wine Wine { get; set; }

		public virtual User Owner { get; set; }

		[Display(Name = "Purchased")]
		public virtual ICollection<Purchase> Purchases { get; set; }

		#endregion Relationships
	}
}