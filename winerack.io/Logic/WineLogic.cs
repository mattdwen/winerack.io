using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using winerack.Models;
using winerack.ViewModels;

namespace winerack.Logic
{
    public class WineLogic
    {
        #region Constructor

        public WineLogic(ApplicationDbContext context)
        {
            db = context;
        }

        #endregion

        #region Declarations

        private readonly ApplicationDbContext db;

        #endregion

        #region Private Methods

        private Region Create_Region(WineEditViewModel model)
        {
            var region = db.Regions
                .Where(r => r.Country == model.Country && r.Name == model.Region)
                .FirstOrDefault();

            if (region == null) {
                region = new Region {
                    Country = model.Country,
                    Name = model.Region
                };

                db.Regions.Add(region);
                db.SaveChanges();
            }

            return region;
        }

        private Vineyard Create_Vineyard(WineEditViewModel model, Region region)
        {
            var vineyard = db.Vineyards
                .Where(v => v.Name == model.Vineyard && v.Region.ID == region.ID)
                .FirstOrDefault();

            if (vineyard == null) {
                vineyard = new Vineyard {
                    Name = model.Vineyard,
                    Region = region
                };

                db.Vineyards.Add(vineyard);
                db.SaveChanges();
            }

            return vineyard;
        }

        #endregion

        #region Public Methods

        public Wine Create_Wine(WineEditViewModel model)
        {
            var region = Create_Region(model);
            var vineyard = Create_Vineyard(model, region);

            var wine = db.Wines
                .Where(w => w.Name == model.WineName
                    && w.RegionID == region.ID
                    && w.Styles.All(s => model.Styles.Contains(s.ID))
                    && w.Varietals.All(v => model.Varietals.Contains(v.ID))
                    && w.VineyardID == vineyard.ID
                    && w.Vintage == model.Vintage)
                .FirstOrDefault();

            if (wine == null) {
                wine = new Wine {
                    Name = model.WineName,
                    RegionID = region.ID,
                    VineyardID = vineyard.ID,
                    Vintage = model.Vintage
                };

                foreach (var styleId in model.Styles) {
                    var style = db.Styles.Find(styleId);
                    wine.Styles.Add(style);
                }

                foreach (var varietalId in model.Varietals) {
                    var varietal = db.Varietals.Find(varietalId);
                    wine.Varietals.Add(varietal);
                }

                db.Wines.Add(wine);
                db.SaveChanges();
            }

            return wine;
        }

        #endregion
    }
}