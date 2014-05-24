using my.winerack.io.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace my.winerack.io.Controllers.API {

	public class VarietalsController : ApiController {
		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Endpoints

		// GET: api/varietals
		public IList<string> GetVarietals() {
			return db.Wines
				.OrderBy(w => w.Varietal)
				.Select(w => w.Varietal)
				.Distinct()
				.ToList();
		}

		#endregion Endpoints
	}
}