using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RogueFit.Data;
using RogueFit.ViewModels;

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
            var viewModel = new ShopViewModel
            {
                Categories = dbContext.Categories.Include(c => c.SubCategories).ToList(),
                SubCategories = dbContext.SubCategories.Include(sc => sc.Tags).ToList(),
                Tags = dbContext.Tags.ToList(),
                Products = dbContext.Products.Include(p => p.Tag).ThenInclude(t => t.SubCategory).ThenInclude(sc => sc.Category).ToList()
            };

            return View(viewModel);
        }
    }
}
