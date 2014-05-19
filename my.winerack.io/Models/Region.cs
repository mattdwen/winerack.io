using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace my.winerack.io.Models {
	public class Region {

		#region Properties

		public int ID { get; set; }

		[Required]
		public string Country { get; set; }

		[Required]
		public string Name { get; set; }

		#endregion

	}
}