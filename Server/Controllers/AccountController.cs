using Classes.DataBase;
using Classes.Models;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {


            private readonly datacontext _context;

            public AccountController(datacontext context)
            {
                _context = context;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register(User user)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Здесь можно добавить логику для хэширования пароля перед сохранением

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("Регистрация успешна!");
            }
        }


   
}
