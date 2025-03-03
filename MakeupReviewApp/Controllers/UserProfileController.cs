using Microsoft.AspNetCore.Mvc;

namespace MakeupReviewApp.Controllers
{
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
