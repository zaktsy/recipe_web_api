using System;
using System.Collections.Generic;

namespace recipe_web_api.Models
{
    public partial class Meal
    {
        public Meal()
        {
            Recipes = new HashSet<Recipe>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
