using winerack.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace winerack.Controllers.API {

	public class VineyardsController : ApiController {
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: api/Vineyards
		public IList<Vineyard> GetVineyards() {
			return db.Vineyards.ToList();
		}

		// GET: api/Vineyards/5
		[ResponseType(typeof(Vineyard))]
		public async Task<IHttpActionResult> GetVineyard(int id) {
			Vineyard vineyard = await db.Vineyards.FindAsync(id);
			if (vineyard == null) {
				return NotFound();
			}

			return Ok(vineyard);
		}

		// PUT: api/Vineyards/5
		[ResponseType(typeof(void))]
		public async Task<IHttpActionResult> PutVineyard(int id, Vineyard vineyard) {
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			if (id != vineyard.ID) {
				return BadRequest();
			}

			db.Entry(vineyard).State = EntityState.Modified;

			try {
				await db.SaveChangesAsync();
			} catch (DbUpdateConcurrencyException) {
				if (!VineyardExists(id)) {
					return NotFound();
				} else {
					throw;
				}
			}

			return StatusCode(HttpStatusCode.NoContent);
		}

		// POST: api/Vineyards
		[ResponseType(typeof(Vineyard))]
		public async Task<IHttpActionResult> PostVineyard(Vineyard vineyard) {
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			db.Vineyards.Add(vineyard);
			await db.SaveChangesAsync();

			return CreatedAtRoute("DefaultApi", new { id = vineyard.ID }, vineyard);
		}

		// DELETE: api/Vineyards/5
		[ResponseType(typeof(Vineyard))]
		public async Task<IHttpActionResult> DeleteVineyard(int id) {
			Vineyard vineyard = await db.Vineyards.FindAsync(id);
			if (vineyard == null) {
				return NotFound();
			}

			db.Vineyards.Remove(vineyard);
			await db.SaveChangesAsync();

			return Ok(vineyard);
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool VineyardExists(int id) {
			return db.Vineyards.Count(e => e.ID == id) > 0;
		}
	}
}