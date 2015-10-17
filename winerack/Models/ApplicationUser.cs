using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNet.Identity.EntityFramework;

namespace winerack.Models
{
  // Add profile data for application users by adding properties to the ApplicationUser class
  public class ApplicationUser : IdentityUser
  {
    #region Properties

    [Display(Name = "First Name")]
    [MaxLength(255)]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    [MaxLength(255)]
    public string LastName { get; set; }

    [Display(Name = "Member since")]
    public DateTime CreatedOn { get; set; }

    [MaxLength(255)]
    public string Location { get; set; }

    [MaxLength(255)]
    public string Country { get; set; }

    public Guid? ImageID { get; set; }

    #endregion Properties

    #region Magic Properties

    [NotMapped]
    public string Name => $"{FirstName} {LastName}";

    [NotMapped]
    public string LocationDescription
    {
      get
      {
        if (string.IsNullOrWhiteSpace(Country))
        {
          return "Unknown";
        }
        var countryID = new RegionInfo(Country).TwoLetterISORegionName;
        return (string.IsNullOrWhiteSpace(Location)) ? countryID : Location + ", " + countryID;
      }
    }

    #endregion Magic Properties

    #region Relationships

    public virtual ICollection<Credentials> Credentials { get; set; }

    public virtual ICollection<Friend> Following { get; set; }

    public virtual ICollection<Friend> Followers { get; set; }

    #endregion Relationships
  }
}