using Classes.DataBase;
using Classes.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Classes.EmailService;
using System.Net;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly datacontext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(datacontext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = new User { Email = model.Email, UserName = model.UserName, Role = model.Role };
            var result = await _userManager.CreateAsync(user, model.PasswordHash);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Code: {error.Code}, Description: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
                
            
            //Двухфакторка


                //if (result.Succeeded)
                //{
                //    Console.WriteLine(user.Id.ToString());
                //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //    var callbackUrl = Url.Action(
                //        "ConfirmEmail",
                //        "Account",
                //        new { userId = user.Id.ToString(), code = WebUtility.UrlEncode(code) },
                //        protocol: HttpContext.Request.Scheme);
                //    callbackUrl = callbackUrl.Replace(';', '&');
                //    EmailService emailService = new EmailService();
                //    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                //        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

            //    return Ok("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
            //}
            //else
            //{
            //    foreach (var error in result.Errors)
            //    {
            //        Console.WriteLine($"Code: {error.Code}, Description: {error.Description}");
            //        ModelState.AddModelError(string.Empty, error.Description);
            //    }
            //    return BadRequest(ModelState);
            //}

        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return BadRequest("User ID and code are required.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            code = WebUtility.UrlDecode(code); // Декодирование токена
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Ok("Email confirmed successfully!");
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] User model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest("Invalid username or password.");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest("Вы не подтвердили свой email.");
            }

            var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.PasswordHash);
            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                // Логика успешного входа, например, создание токена
                return Ok("Вход успешен!");
            }
            else
            {
                return BadRequest("Invalid username or password.");
            }
        }
    }


}
