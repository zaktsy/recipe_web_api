using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipe_web_api.Models;

namespace recipe_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListsController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(int id)
        {
            using (var context = new recipesdbContext())
            {
                var list = await context.ShoppingLists.Where(c => c.Userid == id).ToListAsync();
                return Ok(list);
            }
        }
        [HttpGet]
        public async Task<IActionResult> BuyProduct(int userId, int productId) 
        {
            using(var context = new recipesdbContext())
            {
                var list = await context.ShoppingLists.Where(c => c.Userid == userId).Where(c =>c.Productid == productId).FirstOrDefaultAsync();
                var fridge = await context.Fridges.Where(c => c.Userid == userId).Where(c => c.Productid == productId).FirstOrDefaultAsync();
                
                if (fridge != null && fridge.Measureid == fridge.Measureid)
                {
                    fridge.Amount += list.Amount;
                }
                else
                {
                    fridge = new Fridge()
                    {
                        Measureid = list.Measureid,
                        Amount = list.Amount,
                        Productid = list.Productid,
                        Userid = userId,
                    };
                    context.Fridges.Add(fridge);
                    context.ShoppingLists.Remove(list);
                }
                context.SaveChanges();
            }
            return Ok("успешно");
        }
    }
}
