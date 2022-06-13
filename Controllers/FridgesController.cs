using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipe_web_api.Models;

namespace recipe_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FridgesController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(int id)
        {
            using (var context = new recipesdbContext())
            {
                var list = await context.Fridges.Where(c => c.Userid == id).ToListAsync();
                return Ok(list);
            }
        }
    }
}
