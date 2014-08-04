using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace winerack.Models {

	public class ActivityNotification {

		#region Declarations

		[Key, Column(Order=0)]
		[ForeignKey("Activity")]
		public int ActivityID { get; set; }

		[Key, Column(Order=1)]
		public string UserID { get; set; }

		#endregion

		#region Relationships

		public Activity Activity { get; set; }

		#endregion

	}

}