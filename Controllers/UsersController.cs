using Microsoft.AspNetCore.Mvc;
using recipe_web_api.Infrastructure.RequestParametrs;
using recipe_web_api.Models;
using Microsoft.EntityFrameworkCore;
using recipe_web_api.Infrastructure.Responses;
using Newtonsoft.Json;

namespace recipe_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using (var context = new recipesdbContext())
            {
                var user = await context.Users.Where(a => a.Id == id).FirstOrDefaultAsync();
                Console.WriteLine("Пришел запрос");
                return Ok(user);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAuth([FromQuery] UserAuthParametrs param)
        {
            using (var context = new recipesdbContext())
            {
                var user = context.Users.Where(u => u.Email == param.Email).FirstOrDefault();
                var response = new UserAuthResponce();
                if (user == null) 
                {
                    user = new User()
                    {
                        Email = param.Email,
                        Password = param.Password,
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                    
                    user = context.Users.Where(u => u.Email == param.Email).FirstOrDefault();
                    response.AuthStatus = AuthEnum.Ok;
                    response.UserId = user.Id;

                    return Ok(response);
                }
                else if (user.Password != param.Password) 
                { 
                    response.AuthStatus = AuthEnum.WrongPassword; 
                }
                else 
                { 
                    response.AuthStatus = AuthEnum.Ok;
                    response.UserId = user.Id;
                }
                return Ok(response);
            }
        }
    }
}
