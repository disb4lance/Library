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


            if (EmailExists(user.email))
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return BadRequest(ModelState);
            }
            if (NameExists(user.name))
            {
                ModelState.AddModelError("name", "name is already taken.");
                return BadRequest(ModelState);
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Регистрация успешна!");
        }

        private bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.email.Equals(email));
        }
        private bool NameExists(string name)
        {
            return _context.Users.Any(u => u.name.Equals(name));
        }
    }
   
}
