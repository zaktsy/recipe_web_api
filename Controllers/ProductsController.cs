using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipe_web_api.Infrastructure.RequestParametrs;
using recipe_web_api.Models;
using recipe_web_api.Wrappers;

namespace recipe_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using(var context = new recipesdbContext())
            {
                var product = await context.Products.Where(a => a.Id == id).FirstOrDefaultAsync();
                Console.WriteLine("Пришел запрос");
                return Ok(product);
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductParametrs param)
        {
            using (var context = new recipesdbContext())
            {
                var list = await context.Products.ToListAsync();
                var response = PagedList<Product>.ToPagedList(list, param.PageNumber, param.PageSize);
                return Ok(response);
            }
        }
    }
}
