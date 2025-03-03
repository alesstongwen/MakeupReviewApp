using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using MakeupReviewApp.Models;
using MakeupReviewApp.Repositories;

namespace MakeupReviewApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly MockUserRepository _userRepo = new MockUserRepository();

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User loginUser)
        {
            var user = _userRepo.ValidateUser(loginUser.Email, loginUser.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.FullName ?? "Unknown User"), // ✅ Ensures ClaimTypes.Name is set
        new Claim(ClaimTypes.Email, user.Email)
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            Console.WriteLine($"[DEBUG] User logged in: {user.FullName}");

            return RedirectToAction("Index", "Home");
        }




        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
