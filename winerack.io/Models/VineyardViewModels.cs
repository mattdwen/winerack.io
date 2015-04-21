using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace winerack.Models.VineyardViewModels
{
    public class VineyardEditVM
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string Country { get; set; }
    }
}