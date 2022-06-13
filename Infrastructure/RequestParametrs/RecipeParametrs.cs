using recipe_web_api.Models;

namespace recipe_web_api.Infrastructure.RequestParametrs
{
    public class RecipeParametrs : QueryPaginationParameters
    {
        public string? Name { get; set; }
        public bool? Favs { get; set; }
        public bool? IsFromUserProducts { get; set; }
        public int? KitchenId { get; set; }
        public int? CategoryId { get; set; }
        public int? MealId { get; set; }
        public int? UserId { get; set; }
        public RecipeParametrs(int pageNumber, int pageSize, string? name,
            int? kitchen, int? category, int? meal, int? userId, bool? favs, bool? isFromUserProducts)
        
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Name = name;
            Favs = favs;
            KitchenId = kitchen;
            CategoryId = category;
            MealId = meal;
            UserId = userId;
            IsFromUserProducts = isFromUserProducts;
        }
        public RecipeParametrs(int pageNumber, int pageSize, string? Name,
            int? kitchen, int? category, int? meal)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            this.Name = Name;
            KitchenId = kitchen;
            CategoryId = category;
            MealId = meal;
        }
        public RecipeParametrs()
        {
        }
    }
}
