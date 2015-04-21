using System.Linq;
using System.Web.Http;
using winerack.Models;

namespace winerack.Controllers.API
{
    public class RegionsController : ApiController
    {
        #region Declarations

        private ApplicationDbContext db = new ApplicationDbContext();

        #endregion Declarations

        #region Endpoints

        // GET: api/regions
        public IQueryable<Region> GetRegions(string q)
        {
            var regions = db.Regions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q)) {
                regions = regions.Where(r => r.Name.Contains(q));
            }

            return regions.OrderBy(r => r.Name);
        }

        #endregion Endpoints
    }
}