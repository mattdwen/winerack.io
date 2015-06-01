using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using winerack.Validation;

namespace winerack.Models
{
    public class CellarBottleViewModel
    {
        public int Min { get; set; }

        public int Max { get; set; }
    }

    public class RackBottleViewModel
    {

        #region Properties

        public Bottle Bottle { get; set; }

        [UIHint("Rating")]
        public double? Rating { get; set; }

        #endregion
    }
}

namespace winerack.Models.BottleViewModels
{
    public class ApiBottle
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string Vineyard { get; set; }

        public string Region { get; set; }

        public int? CellarMin { get; set; }

        public int? CellarMax { get; set; }

        public List<string> Styles { get; set; }

        public List<string> Varietals { get; set; }

        public int? Vintage { get; set; }

        public int Purchased { get; set; }

        public int Opened { get; set; }

        public int Remaining { get; set; }
    }
}