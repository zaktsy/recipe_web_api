using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipe_web_api.Models;

namespace recipe_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitchensController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var context = new recipesdbContext())
            {
                var kitchens = await context.Kitchens.ToListAsync();
                return Ok(kitchens);
            }
        }
    }
}
