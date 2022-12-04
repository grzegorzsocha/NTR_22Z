using LibraryManager.Models;
using LibraryManager.Models.Account;
using LibraryManager.Models.Entities;
using LibraryManager.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManager.Controllers
{
    public class AccountController : Controller
    {
        private static string fileName = "users.json";
        private static string booksFileName = "books.json";

        public IActionResult Login(string ReturnUrl = "/")
        {
            var objLoginModel = new LoginViewModel();
            objLoginModel.ReturnUrl = ReturnUrl;
            return View(objLoginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel objLoginModel)
        {
            var users = FileUtils.ReadFromFile<User>(fileName);

            if (ModelState.IsValid)
            {
                var user = users.Where(x => x.Username == objLoginModel.Username && x.Password == objLoginModel.Password).FirstOrDefault();
                if (user == null)
                {
                    ViewBag.Message = "Invalid Credential";
                    return View(objLoginModel);
                }
                else
                {
                    var claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Username)),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.IsAdmin ? Roles.Admin : Roles.User),
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = objLoginModel.RememberLogin
                    });
                    return LocalRedirect(objLoginModel.ReturnUrl);
                }
            }
            return View(objLoginModel);
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }

        public IActionResult Register(string ReturnUrl = "/")
        {
            var objRegisterModel = new RegisterViewModel();
            objRegisterModel.ReturnUrl = ReturnUrl;
            return View(objRegisterModel);
        }

        [Authorize(Policy = Policies.UserOnly)]
        public IActionResult ManageAccount()
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var books = FileUtils.ReadFromFile<Book>(booksFileName);
            var hasBorrowed = books.Where(b => b.User == userName && !b.Leased.HasValue).Any();
            var objModel = new ManageAccountViewModel()
            {
                HasBorrowedBooks = hasBorrowed
            };
            return View(objModel);
        }

        [HttpPost]
        [Authorize]
        [Authorize(Policy = Policies.UserOnly)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(string password)
        {
            var users = FileUtils.ReadFromFile<User>(fileName);
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var user = users.FirstOrDefault(u => u.Username == userName);

            if (user != null)
            {
                if (!user.IsCorrectPassword(password))
                {
                    TempData["ErrorMessage"] = "Invalid Credential";
                    return RedirectToAction("ManageAccount");
                }

                var books = FileUtils.ReadFromFile<Book>(booksFileName);
                var hasBorrowed = books.Where(b => b.User == userName && !b.Leased.HasValue).Any();
                if (hasBorrowed)
                {
                    ViewBag.Message = "User has borrowed books";
                    return RedirectToAction("ManageAccount");
                }
                var reserved = books.Where(b => b.User == userName && !b.Reserved.HasValue).ToList();
                if (reserved.Any())
                {
                    foreach (var book in reserved)
                    {
                        book.CancelReservation();
                    }
                    FileUtils.WriteToFile(booksFileName, books);
                }
                users.Remove(user);
                FileUtils.WriteToFile(fileName, users);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return LocalRedirect("/");
            }
            return RedirectToAction("ManageAccount");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAccount(RegisterViewModel model)
        {
            var users = FileUtils.ReadFromFile<User>(fileName);
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var user = users.FirstOrDefault(u => u.Username == model.Username);

            if (user != null)
            {
                TempData["ErrorMessage"] = "User already exists";
                return RedirectToAction("Register");
            }
            else if (model.Password != model.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords must match";
                return RedirectToAction("Register");
            }

            var newUser = new User()
            {
                Username = model.Username,
                Password = model.Password
            };

            users.Add(newUser);
            FileUtils.WriteToFile(fileName, users);
            return RedirectToAction("Login");
        }
    }
}
