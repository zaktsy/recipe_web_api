using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipe_web_api.Models;

namespace recipe_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasuresController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using (var context = new recipesdbContext())
            {
                var measure = await context.Measures.Where(a => a.Id == id).FirstOrDefaultAsync();
                return Ok(measure);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using (var context = new recipesdbContext())
            {
                var list = await context.Measures.ToListAsync();
                return Ok(list);
            }
        }
    }
}
