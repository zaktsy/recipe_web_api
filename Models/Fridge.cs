using System;
using System.Collections.Generic;

namespace recipe_web_api.Models
{
    public partial class Fridge
    {
        public int Userid { get; set; }
        public int Productid { get; set; }
        public int Measureid { get; set; }
        public int Amount { get; set; }

        public virtual Measure Measure { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
