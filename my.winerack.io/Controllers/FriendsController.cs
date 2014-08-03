using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using winerack.Models;
using System.Linq;
using System;

namespace winerack.Controllers {

	[Authorize]
	public class FriendsController : Controller {

		#region Declarations

		private ApplicationDbContext db = new ApplicationDbContext();

		public enum FriendMessges {
			Followed,
			Unfollowed
		};

		#endregion Declarations

		#region Actions

		#region Index

		// GET: Friends
		public ActionResult Index(FriendMessges? message, string username) {
			var user = db.Users.Find(User.Identity.GetUserId());

			ViewBag.SuccessMessage = message == FriendMessges.Followed ? "You are now following " + username
				: "";

			ViewBag.DangerMessage = message == FriendMessges.Unfollowed ? "You are no longer following " + username
				: "";

			return View(user);
		}

		#endregion Index

		#region Follow

		public ActionResult Follow(string id) {
			var friend = new Friend {
				FolloweeID = id,
				FollowerID = User.Identity.GetUserId(),
				CreatedOn = DateTime.Now
			};

			db.Friends.Add(friend);
			db.SaveChanges();

			var following = db.Users.Find(id);

			return RedirectToAction("", new { message = FriendMessges.Followed, username = following.Name });
		}

		#endregion

		#region Unfollow

		public ActionResult Unfollow(string id) {
			var userId = User.Identity.GetUserId();
			var friend = db.Friends
				.Where(f => f.FolloweeID == id && f.FollowerID == userId)
				.FirstOrDefault();

			var name = friend.Followee.Name;
			db.Friends.Remove(friend);
			db.SaveChanges();

			return RedirectToAction("", new { message = FriendMessges.Unfollowed, username = name });
		}

		#endregion Unfollow

		#endregion Actions
	}
}