using Classes.DataBase;
using Classes.LoginModel;
using Classes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    public class LoansController : Controller
    {
        private datacontext context;




        [HttpPost("AddToLoan")]
        public async Task<IActionResult> AddToLoan([FromBody] LoanDto loanDto)
        {
            if (loanDto == null || string.IsNullOrEmpty(loanDto.UserId) || loanDto.BookId <= 0)
            {
                return BadRequest("Invalid loan data.");
            }

            var loan = new Loan
            {
                UserId = loanDto.UserId,
                BookId = loanDto.BookId,
                LoanDate = loanDto.LoanDate
            };

            context.Loans.Add(loan);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
