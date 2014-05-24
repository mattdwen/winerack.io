using my.winerack.io.Models;
using System.Linq;
using System.Web.Http;

namespace my.winerack.io.Controllers.API {

	public class RegionsController : ApiController {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		#endregion Declarations

		#region Endpoints

		// GET: api/regions
		public IQueryable<Region> GetRegions() {
			return db.Regions;
		}

		#endregion Endpoints
	}
}