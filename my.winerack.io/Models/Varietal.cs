using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace winerack.Models {
    public class Varietal {

        #region Properties

        public int ID { get; set; }

        [Required(ErrorMessage="Name is required")]
        public string Name { get; set; }

        #endregion Properties

        #region Relationships

        [JsonIgnore]
        public virtual ICollection<Wine> Wines { get; set; }

        #endregion Relationships

        #region Public Methods

        public static IEnumerable<Varietal> GetVarietals()
        {
            var context = new ApplicationDbContext();
            return context.Varietals
                .OrderBy(r => r.Name);
        }

        #endregion Public Methods
    }
}