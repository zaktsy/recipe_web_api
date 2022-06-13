using System;
using System.Collections.Generic;

namespace recipe_web_api.Models
{
    public partial class RecipeStep
    {
        public int Recipeid { get; set; }
        public int Stepnumber { get; set; }
        public string? Description { get; set; }

        public virtual Recipe Recipe { get; set; } = null!;
    }
}
