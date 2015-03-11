using winerack.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace winerack.Controllers.API {

	public class VarietalsController : ApiController {
		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Endpoints

		// GET: api/varietals
		public IList<Varietal> GetVarietals() {
			return db.Varietals.OrderBy(v => v.Name).ToList();
		}

		#endregion Endpoints
	}
}