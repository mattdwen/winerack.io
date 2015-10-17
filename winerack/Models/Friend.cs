using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace winerack.Models
{
  public class Friend
  {
    #region Properties

    public int ID { get; set; }

    public DateTime CreatedOn { get; set; }

    [Required]
    [ForeignKey("Followee")]
    public string FolloweeID { get; set; }

    [Required]
    [ForeignKey("Follower")]
    public string FollowerID { get; set; }

    #endregion Properties

    #region Relationships

    public virtual ApplicationUser Followee { get; set; }

    public virtual ApplicationUser Follower { get; set; }

    #endregion Relationships
  }
}