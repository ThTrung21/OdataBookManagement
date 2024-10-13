using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore; // Make sure to include this for async methods
using Microsoft.Extensions.Logging;
using ODataAPI.Models;
using System.Threading.Tasks;

namespace ODataAPI.Controllers
{
    [Route("odata/Books")]
    public class BooksController : ODataController
    {
        private readonly BookDbContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(BookDbContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Get request for all books received.");
            return Ok(_context.Books);
        }

        [EnableQuery]
        [HttpGet("({key})")]
        public IActionResult Get([FromRoute] int key)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == key);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EDM.Book book)
        {
            _logger.LogInformation($"Post request received: {book.Title}, {book.Author}");
            if (book == null)
            {
                return BadRequest("Book cannot be null.");
            }

            // Assuming Id is auto-generated
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return Created($"odata/Books({book.Id})", book); // Return location of the created resource
        }

        [HttpPut("({key})")]
        public async Task<IActionResult> Put([FromRoute] int key, [FromBody] EDM.Book book)
        {
            if (key != book.Id)
            {
                return BadRequest("Book ID mismatch.");
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(key))
                {
                    return NotFound();
                }
                throw; // Re-throw the exception for further handling
            }

            return NoContent();
        }

        [HttpDelete("({key})")]
        public async Task<IActionResult> Delete([FromRoute] int key)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == key);
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
