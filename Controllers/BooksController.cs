using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // Allow anyone to get books list
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _context.Books.Include(b => b.Category).ToListAsync();
            return Ok(books);
        }

        // Allow anyone to get a book by ID
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.Include(b => b.Category).FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null) return NotFound();

            return Ok(book);
        }

        // Other endpoints (POST, PUT, DELETE) could have authorization if needed
        // For example, restrict to Librarian role:

        //[Authorize(Roles = "Librarian")]
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (book == null)
                return BadRequest("Book data is required.");

            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == book.CategoryId);
            if (!categoryExists)
                return BadRequest("Category does not exist.");

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.BookId }, book);
        }

        //[Authorize(Roles = "Librarian")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
                return BadRequest("ID mismatch.");

            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == book.CategoryId);
            if (!categoryExists)
                return BadRequest("Category does not exist.");

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Books.AnyAsync(b => b.BookId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        //[Authorize(Roles = "Librarian")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
