using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipe_web_api.Models;

namespace recipe_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var context = new recipesdbContext())
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(categories);
            }
        }
    }
}
