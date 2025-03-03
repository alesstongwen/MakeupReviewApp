using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MakeupReviewApp.Repositories;

namespace MakeupReviewApp.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly MockUserRepository _userRepo;

        // ✅ Inject MockUserRepository
        public UserProfileController(MockUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult Profile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            // ✅ Ensure user is logged in
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var userProfile = _userRepo.GetUserProfileByEmail(userEmail);
            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            return View(userProfile);
        }

    }
}
