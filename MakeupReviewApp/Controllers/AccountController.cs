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
        private readonly MockUserRepository _userRepo;

        public AccountController(MockUserRepository userRepo)
        {
            _userRepo = userRepo;
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User loginUser)
        {
            if (loginUser == null || string.IsNullOrEmpty(loginUser.Email) || string.IsNullOrEmpty(loginUser.Password))
            {
                ModelState.AddModelError("", "Email and Password are required.");
                return View();
            }

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

        public IActionResult Profile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); 

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var userProfile = _userRepo.GetUserProfileByEmail(userEmail);
            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            Console.WriteLine($"[DEBUG] User.Identity.Name: {User.Identity.Name}");
            Console.WriteLine($"[DEBUG] User Email: {User.FindFirstValue(ClaimTypes.Email)}");

            return View(userProfile);

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User newUser)
        {
            if (string.IsNullOrEmpty(newUser.Email) || string.IsNullOrEmpty(newUser.Password) || string.IsNullOrEmpty(newUser.FullName))
            {
                ModelState.AddModelError("", "All fields are required.");
                return View();
            }

            try
            {
                _userRepo.AddUser(newUser);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            return RedirectToAction("Login");
        }

    }
}
