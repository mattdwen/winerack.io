using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace winerack.Models {
	public class Varietal {

		#region Properties

		public int ID { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public VarietalStyles Style { get; set; }

		#endregion Properties

		#region Relationships

		[JsonIgnore]
		public virtual ICollection<Wine> Wines { get; set; }

		#endregion Relationships
	}

	public enum VarietalStyles {
		Red = 100,
		White = 200,
		Fortified = 300,
		Dessert = 400,
		Sparkling = 500,
		Other = 1000
	}
}