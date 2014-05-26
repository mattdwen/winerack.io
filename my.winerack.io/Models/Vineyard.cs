using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace winerack.Models {

	public class Vineyard {

		#region Properties

		public int ID { get; set; }

		[Required]
		public string Name { get; set; }

		#endregion Properties

		#region Relationships

		[JsonIgnore]
		public virtual ICollection<Wine> Wines { get; set; }

		#endregion Relationships

		#region Public Methods

		public static IEnumerable<Vineyard> GetVineyards() {
			var context = new ApplicationDbContext();
			return context.Vineyards
				.OrderBy(m => m.Name);
		}

		#endregion Public Methods
	}
}