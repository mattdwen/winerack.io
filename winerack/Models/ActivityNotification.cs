using System.ComponentModel.DataAnnotations.Schema;

namespace winerack.Models
{
  public class ActivityNotification
  {
    #region Declarations

    [ForeignKey("Activity")]
    public int ActivityID { get; set; }

    public string UserID { get; set; }

    #endregion

    #region Relationships

    public Activity Activity { get; set; }

    #endregion
  }
}