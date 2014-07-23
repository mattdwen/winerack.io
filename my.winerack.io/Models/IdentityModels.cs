using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace winerack.Models {

	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class User : IdentityUser {

		#region Properties

		public DateTime CreatedOn { get; set; }

		[Display(Name="First Name")]
		[MaxLength(255)]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		[MaxLength(255)]
		public string LastName { get; set; }

		public Guid? ImageID { get; set; }

		#endregion Properties

		#region Relationships

		public virtual ICollection<Credentials> Credentials { get; set; }

		#endregion Relationships

		#region Public Methods

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager) {
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}

		#endregion Public Methods
	}
}