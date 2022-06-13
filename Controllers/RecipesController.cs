using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipe_web_api.Infrastructure.RequestParametrs;
using recipe_web_api.Models;
using recipe_web_api.Wrappers;

namespace recipe_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] RecipeParametrs param)
        {
            using (var context = new recipesdbContext())
            {
                bool NameCondition = param.Name != String.Empty && param.Name != null;
                bool KitchenCondition = param.KitchenId != null && param.KitchenId > 0;
                bool CategoryCondition = param.CategoryId != null && param.CategoryId > 0;
                bool MealCondition = param.MealId != null && param.MealId > 0;

                List<Recipe> UserFavRecipes = new List<Recipe>();

                var AllRecipes = await (from c in context.Recipes where ((!NameCondition || c.Name.ToLower().Contains(param.Name.ToLower())) &&
                                  (!KitchenCondition || c.Kitchenid == param.KitchenId) &&
                                  (!CategoryCondition || c.Categoryid == param.CategoryId) &&
                                  (!MealCondition || c.Mealid == param.MealId))
                                  select c).Include(c => c.ProductRecipes).ToListAsync();


                if(param.Favs != null && param.UserId!= null)
                {
                    if((bool)param.Favs && (int)param.UserId > 0)
                    {
                        var user = await context.Users.Where(c => c.Id == param.UserId).Include(c => c.Recipes).FirstOrDefaultAsync();
                        if (user != null)
                        {
                            UserFavRecipes = user.Recipes.ToList();
                            UserFavRecipes.ForEach(c => c.Users = null);
                            AllRecipes = AllRecipes.Intersect(UserFavRecipes).ToList();
                        }

                    }
                }

                if (param.IsFromUserProducts != null && param.UserId != null)
                {
                    if ((bool)param.IsFromUserProducts && (int)param.UserId > 0)
                    {
                        var user = context.Users.Where(c => c.Id == param.UserId).Include(c => c.Recipes).FirstOrDefault();
                        AllRecipes.ForEach(c => c.Users = null);
                        
                        if (user != null)
                        {
                            List<Fridge> userFridge = new List<Fridge>(context.Fridges.Where(c => c.Userid == user.Id).ToList());
                            List<int> userProdId = new List<int>();
                            foreach (var fr in userFridge)
                            {
                                userProdId.Add(fr.Productid);
                            }
                            for (int i = 0; i < AllRecipes.Count; i++)
                            {
                                bool ok = true;
                                int needOk = AllRecipes[i].ProductRecipes.Count;
                                int okk = 0;
                                foreach (var PrR in AllRecipes[i].ProductRecipes)
                                {
                                    foreach (var prod in userFridge)
                                    {
                                        if ((prod.Productid == PrR.Productid) && (prod.Measureid == PrR.Mesureid) && (prod.Amount >= PrR.Amount))
                                        {
                                            okk++;
                                        }
                                    }
                                }
                                if (okk != needOk) { ok = false; }
                                if (!ok) { AllRecipes.Remove(AllRecipes[i]); i--; }
                            }
                        }

                    }
                }

                foreach (var recipe in AllRecipes)
                {
                    recipe.Meal = (from meal in context.Meals where meal.Id == recipe.Mealid select meal).FirstOrDefault();
                    if (recipe.Meal != null)
                        recipe.Meal.Recipes = null;
                    recipe.Category = (from category in context.Categories where category.Id == recipe.Categoryid select category).FirstOrDefault();
                    if (recipe.Category!= null) 
                        recipe.Category.Recipes = null;
                    recipe.Kitchen = (from kitchen in context.Kitchens where kitchen.Id == recipe.Kitchenid select kitchen).FirstOrDefault();
                    if (recipe.Kitchen != null)
                        recipe.Kitchen.Recipes = null;
                }

                var response = PagedList<Recipe>.ToPagedList(AllRecipes, param.PageNumber, param.PageSize);
                return Ok(response);
            }

        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using (var context = new recipesdbContext())
            {
                var recipe = await context.Recipes.Where(a => a.Id == id).Include(c => c.ProductRecipes).
                    Include(c => c.RecipeSteps).FirstOrDefaultAsync();
                return Ok(recipe);
            }
        }
    }
}
