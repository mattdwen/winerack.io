using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace winerack.Models
{
    public class Style
    {
        #region Properties

        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        #endregion Properties

        #region Relationships

        [JsonIgnore]
        public virtual ICollection<Wine> Wines { get; set; }

        #endregion Relationships
    }
}