using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace winerack.Models
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    #region Properties

    public DbSet<Activity> Activities { get; set; }

    public DbSet<ActivityNotification> ActivityNotifications { get; set; }

    public DbSet<Bottle> Bottles { get; set; }

    public DbSet<Credentials> Credentials { get; set; }

    public DbSet<Friend> Friends { get; set; }

    public DbSet<Opening> Openings { get; set; }

    public DbSet<Purchase> Purchases { get; set; }

    public DbSet<Region> Regions { get; set; }

    public DbSet<StoredBottle> StoredBottles { get; set; }

    public DbSet<Style> Styles { get; set; }

    public DbSet<Tasting> Tastings { get; set; }

    public DbSet<Varietal> Varietals { get; set; }

    public DbSet<Vineyard> Vineyards { get; set; }

    public DbSet<Wine> Wines { get; set; }

    #endregion Properties

    #region Overrides

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<ActivityNotification>().HasKey(t => new {t.ActivityID, t.UserID});
    }

    #endregion Overrides
  }
}