using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipe_web_api.Infrastructure.Responses;
using recipe_web_api.Models;

namespace recipe_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CookItController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetById(int userId, int recipeId)
        {
            using (var context = new recipesdbContext())
            {
                CookItStatus status = CookItStatus.Error;

                var user = context.Users.Where(u => u.Id == userId).Include(c => c.Recipes)
                    .Include(c => c.Fridges).FirstOrDefault();
                var recipe = context.Recipes.Where(u => u.Id == recipeId)
                    .Include(c => c.ProductRecipes).FirstOrDefault();
                var IsUserHaveRecipe = user.Recipes.Contains(recipe);


                List<ProductRecipe> neededProducts = new List<ProductRecipe>(recipe.ProductRecipes);
                List<Fridge> userFridge = new List<Fridge>(user.Fridges);

                userFridge.OrderBy(n => n.Productid);
                neededProducts.OrderBy(n => n.Productid);

                bool allProducts = false;
                int startIndex = 0;
                List<Fridge> neededProdForRecipe = new List<Fridge>();

                foreach (var need in neededProducts)
                {
                    int i = startIndex;
                    bool isEqual = false;
                    while (i < userFridge.Count && !isEqual)
                    {
                        if ((need.Productid == userFridge[i].Productid) && (need.Mesureid == userFridge[i].Measureid) && (need.Amount <= userFridge[i].Amount))
                        {
                            isEqual = true;
                            startIndex++;
                        }
                        i++;
                    }
                    if (!isEqual)
                    {
                        var tempFridge = new Fridge();
                        tempFridge.Measureid = (int)need.Mesureid;
                        tempFridge.Productid = need.Productid;
                        tempFridge.Amount = (int)need.Amount;
                        tempFridge.Userid = user.Id;
                        neededProdForRecipe.Add(tempFridge);
                    }
                }

                if (neededProdForRecipe.Count != 0)
                {
                    List<ShoppingList> userShoppingList = new List<ShoppingList>(
                        context.ShoppingLists.Where(c => c.Userid == user.Id).ToList());
                    userShoppingList.OrderBy(n => n.Productid);
                    foreach (var prod in neededProdForRecipe)
                    {
                        bool added = false;
                        foreach (var sh in userShoppingList)
                        {
                            if ((prod.Productid == sh.Productid) && (prod.Measureid == sh.Measureid))
                            {
                                sh.Amount += prod.Amount;
                                context.SaveChanges();
                                added = true;
                            }

                        }
                        if (!added)
                        {
                            ShoppingList shop = new ShoppingList();
                            shop.Amount = prod.Amount;
                            shop.Userid = user.Id;
                            shop.Productid = prod.Productid;
                            shop.Measureid = prod.Measureid;
                            context.ShoppingLists.Add(shop);
                            context.SaveChanges();
                        }
                    }
                    if (!IsUserHaveRecipe)
                    {
                        user.Recipes.Add(context.Recipes.Where(c => c.Id == recipe.Id).FirstOrDefault());
                        context.SaveChanges();
                    }
                    status = CookItStatus.AddedInFavs;
                }
                else
                {
                    foreach (var fr in userFridge)
                    {
                        foreach (var pr in neededProducts)
                        {
                            if ((fr.Productid == pr.Productid) && (fr.Measureid == pr.Mesureid))
                            {
                                fr.Amount -= (int)pr.Amount;
                                if (fr.Amount == 0)
                                {
                                    context.Fridges.Remove(fr);
                                }
                                context.SaveChanges();
                            }
                        }
                    }
                    if (IsUserHaveRecipe)
                    {
                        user.Recipes.Remove(context.Recipes.Where(c => c.Id == recipe.Id).FirstOrDefault());
                        context.SaveChanges();
                    }
                    status = CookItStatus.Cooked;
                }
                return Ok(status);
            }
        }
    }
}
