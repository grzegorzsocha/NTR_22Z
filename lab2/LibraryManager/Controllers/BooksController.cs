using LibraryManager.Database;
using LibraryManager.Models;
using LibraryManager.Models.Books;
using LibraryManager.Models.Entities;
using LibraryManager.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Controllers
{
    [Authorize]
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


        public async Task<IActionResult> Index(string searchString)
        {
            var bookQuery = context.Books.AsQueryable();
            bookQuery = SearchBooks(bookQuery, searchString);
            var books = await LoadBooks(bookQuery);

            return View(new IndexViewModel()
            {
                Books = books
            });
        }

        public async Task<IActionResult> Reservations(string searchString)
        {
            var bookQuery = context.Books.Where(s => s.Reserved.HasValue);
            bookQuery = SearchBooks(bookQuery, searchString);
            var books = await LoadBooks(bookQuery);

            return View(new IndexViewModel()
            {
                Books = books
            });
        }

        public async Task<IActionResult> MyReservations(string searchString)
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var bookQuery = context.Books.Where(s => s.Username == userName && s.Reserved.HasValue);
            bookQuery = SearchBooks(bookQuery, searchString);
            var books = await LoadBooks(bookQuery);

            return View(new IndexViewModel()
            {
                Books = books
            });
        }

        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> Borrowings(string searchString)
        {
            var bookQuery = context.Books.Where(s => s.Leased.HasValue);
            bookQuery = SearchBooks(bookQuery, searchString);
            var books = await LoadBooks(bookQuery);

            return View(new IndexViewModel()
            {
                Books = books
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.UserOnly)]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book != null)
            {
                book.CancelReservation();
                await context.SaveChangesAsync();
            }
            return RedirectToAction("MyReservations");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.UserOnly)]
        public async Task<IActionResult> MakeReservation(int id)
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book != null && book.CanReserve())
            {
                book.MakeReservation(userName);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> BorrowBook(int id)
        {
            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);
            var userName = book.Username;

            if (book != null && book.CanLease())
            {
                book.MakeLease(userName);
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Reservations");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book != null)
            {
                book.ReturnLease();
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Borrowings");
        }
    }
}
