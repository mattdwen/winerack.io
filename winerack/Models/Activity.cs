using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace winerack.Models
{
  public class Activity
  {
    #region Properties

    public int ID { get; set; }

    [Required]
    public DateTime OccuredOn { get; set; }

    [Required]
    [ForeignKey("Actor")]
    public string ActorID { get; set; }

    [Required]
    public int ObjectID { get; set; }

    [ForeignKey("Wine")]
    public int? WineID { get; set; }

    [Required]
    public ActivityVerbs Verb { get; set; }

    #endregion Properties

    #region Relationships

    public virtual ApplicationUser Actor { get; set; }

    public virtual ICollection<ActivityNotification> Notifications { get; set; } = new List<ActivityNotification>();

    public virtual Wine Wine { get; set; }

    #endregion Relationships
  }

  public enum ActivityVerbs
  {
    Purchased,
    Opened,
    Tasted
  }
}