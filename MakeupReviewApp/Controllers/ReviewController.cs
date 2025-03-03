using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MakeupReviewApp.Models;
using MakeupReviewApp.Repositories;

public class ReviewController : Controller
{
    private readonly MockReviewRepository _reviewRepo = new MockReviewRepository();

    [Authorize]
    public IActionResult Create(int productId)
    {
        return View(new Review { ProductId = productId });
    }
    [HttpPost]
    [Authorize]
    public IActionResult Create(Review review)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);

        // ✅ Manually assign UserName before validation
        review.UserName = userName ?? "Unknown User"; // Avoid validation failure

        if (string.IsNullOrEmpty(review.UserName) || review.UserName == "Unknown User")
        {
            ModelState.AddModelError("UserName", "UserName is required but was not found.");
        }

        // 🔥 Debugging: Log UserName and ModelState Errors
        Console.WriteLine($"[DEBUG] UserName Assigned: {review.UserName}");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("[ERROR] ModelState is not valid.");
            foreach (var error in ModelState)
            {
                foreach (var subError in error.Value.Errors)
                {
                    Console.WriteLine($"[ERROR] {error.Key}: {subError.ErrorMessage}");
                }
            }

            return View(review); // Return the form with validation messages
        }

        _reviewRepo.AddReview(review);
        Console.WriteLine($"[DEBUG] New Review by {review.UserName} - {review.Comment}");

        return RedirectToAction("Details", "Product", new { id = review.ProductId });
    }

}
