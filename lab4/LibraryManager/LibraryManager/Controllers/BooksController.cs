using LibraryManager.Core;
using LibraryManager.Database;
using LibraryManager.Models.Entities;
using LibraryManager.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Controllers
{
    [Authorize]
    [Route("books")]
    public class BooksController : Controller
    {
        private readonly DataContext context;

        public BooksController(DataContext context)
        {
            this.context = context;
        }

        private static IQueryable<Book> SearchBooks(IQueryable<Book> books, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                return books
                        .Where(s => s.Title.ToLower().Contains(searchString)
                        || s.Author.ToLower().Contains(searchString)
                        || s.Publisher.ToLower().Contains(searchString));
            }
            return books;
        }

        private static Task<List<Book>> LoadBooks(IQueryable<Book> books)
        {
            return books
                .OrderBy(s => s.Title)
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var bookQuery = context.Books.AsQueryable();
            bookQuery = SearchBooks(bookQuery, searchString);
            var books = await LoadBooks(bookQuery);

            return Ok(books);
        }

        [HttpGet("/reservations")]
        public async Task<IActionResult> Reservations([FromQuery] string searchString)
        {
            var bookQuery = context.Books.Where(s => s.Reserved.HasValue);
            bookQuery = SearchBooks(bookQuery, searchString);
            var books = await LoadBooks(bookQuery);

            return Ok(books);
        }

        [HttpGet("/myreservations")]
        public async Task<IActionResult> MyReservations(string searchString)
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var bookQuery = context.Books.Where(s => s.Username == userName && s.Reserved.HasValue);
            bookQuery = SearchBooks(bookQuery, searchString);
            var books = await LoadBooks(bookQuery);

            return Ok(books);
        }

        [Authorize(Policy = Policies.AdminOnly)]
        [HttpGet("/borrowings")]
        public async Task<IActionResult> Borrowings(string searchString)
        {
            var bookQuery = context.Books.Where(s => s.Leased.HasValue);
            bookQuery = SearchBooks(bookQuery, searchString);
            var books = await LoadBooks(bookQuery);

            return Ok(books);
        }

        [HttpDelete("{id}/reservations")]
        [Authorize(Policy = Policies.UserOnly)]
        public async Task<IActionResult> CancelReservation(int id, [FromQuery] uint rowVersion)
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book != null && book.IsReserved() && book.Username == userName)
            {
                book.CancelReservation();
                context.Entry(book).Property("RowVersion").OriginalValue = rowVersion;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Reservation has already been changed by someone else");
                }
            }
            else
            {
                return BadRequest("Reservation not found");
            }

            return Ok();
        }

        [HttpPost("{id}/reservations")]
        [Authorize(Policy = Policies.UserOnly)]
        public async Task<IActionResult> MakeReservation(int id, [FromQuery] uint rowVersion)
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book != null && book.CanReserve())
            {
                book.MakeReservation(userName);
                context.Entry(book).Property("RowVersion").OriginalValue = rowVersion;
                try
                {
                    await context.SaveChangesAsync();
                    //await Task.Delay(5 * 1000); // For testing concurency
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest("Reservation has already been made by someone else");
                }
            }
            else
            {
                return BadRequest("Cannot reserve this book");
            }

            return Ok();
        }

        [HttpPost("{id}/borrowings")]
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> BorrowBook(int id, [FromQuery] uint rowVersion)
        {
            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);
            var userName = book.Username;

            if (book != null && book.CanLease())
            {
                book.MakeLease(userName);
                context.Entry(book).Property("RowVersion").OriginalValue = rowVersion;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Book has been borrowed by someone else");
                }
            }
            else
            {
                return BadRequest("Cannot borrow this book");
            }

            return Ok();
        }

        [HttpDelete("{id}/borrowings")]
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> ReturnBook(int id, uint rowVersion)
        {
            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book != null && book.IsLeased())
            {
                book.ReturnLease();
                context.Entry(book).Property("RowVersion").OriginalValue = rowVersion;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest("Reservation has been made by someone else");
                }
            }
            else
            {
                return BadRequest("Book not found");
            }

            return Ok();
        }
    }
}
