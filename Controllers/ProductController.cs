using Microsoft.AspNetCore.Mvc;
using RogueFit.Data;

namespace RogueFit.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var products = dbContext.Products.ToList();
            return View(products);
        }
    }
}
