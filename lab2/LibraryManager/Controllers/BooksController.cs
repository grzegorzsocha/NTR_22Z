using LibraryManager.Models;
using LibraryManager.Models.Books;
using LibraryManager.Models.Entities;
using LibraryManager.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private static string fileName = "books.json";

        private List<Book> SearchBooks(List<Book> books, string searchString)
        {
            searchString = searchString.ToLower();
            return books
                    .Where(s => s.Title.ToLower().Contains(searchString)
                    || s.Author.ToLower().Contains(searchString)
                    || s.Publisher.ToLower().Contains(searchString))
                    .ToList();
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var books = FileUtils.ReadFromFile<Book>(fileName);

            if (!string.IsNullOrEmpty(searchString))
            {
                books = SearchBooks(books, searchString);
            }
            return View(new IndexViewModel()
            {
                Books = books
            });
        }

        public async Task<IActionResult> Reservations(string searchString)
        {
            var books = FileUtils.ReadFromFile<Book>(fileName);
            books = books.Where(s => !s.Reserved.HasValue).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                books = SearchBooks(books, searchString);
            }
            return View(new IndexViewModel()
            {
                Books = books
            });
        }

        public async Task<IActionResult> MyReservations(string searchString)
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var books = FileUtils.ReadFromFile<Book>(fileName);
            books = books.Where(s => s.User == userName && !s.Reserved.HasValue).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                books = SearchBooks(books, searchString);
            }
            return View(new IndexViewModel()
            {
                Books = books
            });
        }

        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> Borrowings(string searchString)
        {
            var books = FileUtils.ReadFromFile<Book>(fileName);
            books = books.Where(s => !s.Leased.HasValue).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                books = SearchBooks(books, searchString);
            }
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
            var books = FileUtils.ReadFromFile<Book>(fileName);
            var book = books.FirstOrDefault(x => x.Id == id);

            if (book != null)
            {
                book.CancelReservation();
                FileUtils.WriteToFile<Book>(fileName, books);
            }
            return RedirectToAction("MyReservations");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.UserOnly)]
        public async Task<IActionResult> MakeReservation(int id)
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var books = FileUtils.ReadFromFile<Book>(fileName);
            var book = books.FirstOrDefault(x => x.Id == id);

            if (book != null && book.CanReserve())
            {
                book.MakeReservation(userName);
                FileUtils.WriteToFile<Book>(fileName, books);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> BorrowBook(int id)
        {
            var books = FileUtils.ReadFromFile<Book>(fileName);
            var book = books.FirstOrDefault(x => x.Id == id);
            var userName = book.User;

            if (book != null && book.CanLease())
            {
                book.MakeLease(userName);
                FileUtils.WriteToFile<Book>(fileName, books);
            }
            return RedirectToAction("Reservations");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Policies.AdminOnly)]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var books = FileUtils.ReadFromFile<Book>(fileName);
            var book = books.FirstOrDefault(x => x.Id == id);

            if (book != null)
            {
                book.ReturnLease();
                FileUtils.WriteToFile<Book>(fileName, books);
            }
            return RedirectToAction("Borrowings");
        }
    }
}
