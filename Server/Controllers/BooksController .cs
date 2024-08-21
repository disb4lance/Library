using Classes.DataBase;
using Classes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Filters;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly datacontext _context;
        public BooksController(datacontext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.Include(b => b.Genres).ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.Include(b => b.Genres)
                                            .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooks(string title, string author, string genre)
        {
            var query = _context.Books.Include(b => b.Genres).AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(b => b.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(b => b.Author.Contains(author));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(b => b.Genres.Any(g => g.Name.Contains(genre)));
            }

            return await query.ToListAsync();
        }

        // POST: api/Books
        //[ServiceFilter(typeof(ValidateTokenFilter))]
        //[Authorize(Policy = "AdminOnly")]
        [HttpPost("AddBook")]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(string.Join("; ", errors));
            }
            foreach (var genre in book.Genres)
            {
                // Пытаемся найти существующий жанр по имени
                var existingGenre = await _context.Genres
                    .FirstOrDefaultAsync(g => g.Name == genre.Name);

                if (existingGenre == null)
                {
                    // Добавляем новый жанр, если его нет в базе данных
                    _context.Genres.Add(genre);
                }
                else
                {
                    // Связываем существующий жанр с книгой
                    genre.Id = existingGenre.Id;
                }
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            return Ok();
        }

        

        // PUT: api/Books/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
