using LibraryManager.Core;
using LibraryManager.Database;
using LibraryManager.Models.DTOs;
using LibraryManager.Models.Entities;
using LibraryManager.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace LibraryManager.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly DataContext context;

        public AccountController(DataContext context)
        {
            this.context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel objLoginModel)
        {
            var user = await context.Users
                .Where(x => x.Username == objLoginModel.Username && x.Password == objLoginModel.Password)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("Invalid Credential");
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

                return Ok();
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserData()
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var isAdmin = HttpContextUtils.GetCurrentUserIsAdmin(HttpContext);
            var userData = new UserData() { Username = userName, IsAdmin = isAdmin };
            return Ok(userData);
        }


        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetAccountInfo()
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var books = await context.Books.Where(b => b.Username == userName && !b.Leased.HasValue).AsNoTracking().CountAsync();
            return Ok(new AccountInfo() { NumberOfBorrowings = books });
        }

        [HttpPut]
        [Authorize]
        [Authorize(Policy = Policies.UserOnly)]
        public async Task<IActionResult> DeleteAccount([FromBody] ManageAccountModel model)
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == userName);

            if (user != null)
            {
                if (!user.IsCorrectPassword(model.Password))
                {
                    return BadRequest("Invalid Credential");
                }

                var books = await context.Books.ToListAsync();
                var hasBorrowed = books.Where(b => b.Username == userName && !b.Leased.HasValue).Any();

                if (hasBorrowed)
                {
                    return BadRequest("User has borrowed books");
                }

                var reserved = books.Where(b => b.Username == userName && !b.Reserved.HasValue).ToList();

                if (reserved.Any())
                {
                    foreach (var book in reserved)
                    {
                        book.CancelReservation();
                    }
                    await context.SaveChangesAsync();
                }

                context.Users.Remove(user);
                await context.SaveChangesAsync();

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAccount([FromBody] RegisterModel model)
        {
            var userName = HttpContextUtils.GetCurrentUsername(HttpContext);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user != null)
            {
                return BadRequest("User already exists");
            }

            var newUser = new User()
            {
                Username = model.Username,
                Password = model.Password
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
