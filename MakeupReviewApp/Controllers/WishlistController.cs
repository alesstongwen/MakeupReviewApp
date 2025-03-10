using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MakeupReviewApp.Repositories;

namespace MakeupReviewApp.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly MockUserRepository _userRepo;
        private readonly MockProductRepository _productRepo;

        public WishlistController(MockUserRepository userRepo, MockProductRepository productRepo)
        {
            _userRepo = userRepo;
            _productRepo = productRepo;
        }

        [HttpPost]
        public IActionResult AddToWishlist(int productId)
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

            var product = _productRepo.GetProductById(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            if (!userProfile.Wishlist.Any(p => p.Id == productId))
            {
                userProfile.Wishlist.Add(product);
            }

            return RedirectToAction("Profile", "UserProfile");
        }

        [HttpPost]
        public IActionResult RemoveFromWishlist(int productId)
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

            var product = userProfile.Wishlist.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                userProfile.Wishlist.Remove(product);
            }

            return RedirectToAction("Profile", "UserProfile");
        }
    }
}