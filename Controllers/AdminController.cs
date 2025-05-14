using Microsoft.AspNetCore.Mvc;

namespace RogueFit.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
