using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LoansController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            var loans = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .ToListAsync();

            return Ok(loans);
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(l => l.LoanId == id);

            if (loan == null) return NotFound();

            return Ok(loan);
        }

        // POST: api/Loans
        [HttpPost]
        public async Task<ActionResult<Loan>> CreateLoan(Loan loan)
        {
            if (loan == null)
                return BadRequest("Loan data is required.");

            var bookExists = await _context.Books.AnyAsync(b => b.BookId == loan.BookId);
            var memberExists = await _context.Members.AnyAsync(m => m.MemberId == loan.MemberId);

            if (!bookExists)
                return BadRequest($"Book with ID {loan.BookId} does not exist.");

            if (!memberExists)
                return BadRequest($"Member with ID {loan.MemberId} does not exist.");

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoan), new { id = loan.LoanId }, loan);
        }

        // PUT: api/Loans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, Loan loan)
        {
            if (id != loan.LoanId)
                return BadRequest("Loan ID mismatch.");

            var bookExists = await _context.Books.AnyAsync(b => b.BookId == loan.BookId);
            var memberExists = await _context.Members.AnyAsync(m => m.MemberId == loan.MemberId);

            if (!bookExists)
                return BadRequest($"Book with ID {loan.BookId} does not exist.");

            if (!memberExists)
                return BadRequest($"Member with ID {loan.MemberId} does not exist.");

            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Loans.AnyAsync(l => l.LoanId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) return NotFound();

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
