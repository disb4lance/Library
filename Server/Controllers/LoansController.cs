using Classes.DataBase;
using Classes.LoginModel;
using Classes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : Controller
    {
        private datacontext context;

        public LoansController(datacontext context)
        {
            this.context = context;
        }

        [HttpGet("GetLoansByUserId")]
        public async Task<IActionResult> GetLoansByUserId([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

            try
            {
                var loans = await context.Loans
                    .Where(l => l.UserId == userId)
                    .Include(l => l.Book)
                        .ThenInclude(b => b.Genres)
                    .ToListAsync();

                if (loans == null || loans.Count == 0)
                {
                    return NotFound($"No loans found for UserId: {userId}");
                }

                var loansDto = loans.Select(l => new Classes.LoginModel.LoanDto
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    LoanDate = l.LoanDate,
                    ReturnDate = l.ReturnDate,
                    IsReturned = l.IsReturned,
                    Book = new BookDto
                    {
                        Id = l.Book.Id,
                        Title = l.Book.Title,
                        Author = l.Book.Author,
                        ISBN = l.Book.ISBN,
                        PublishedDate = l.Book.PublishedDate,
                        Genres = l.Book.Genres.Select(g => g.Name).ToList()
                    }
                }).ToList();

                return Ok(loansDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPost("AddToLoan")]
        public async Task<IActionResult> AddToLoan([FromBody] Classes.LoginModel.Loan loanDto)
        {
            if (loanDto == null || string.IsNullOrEmpty(loanDto.UserId) || loanDto.BookId <= 0)
            {
                return BadRequest("Invalid loan data.");
            }

            var loan = new Classes.Models.Loan
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
