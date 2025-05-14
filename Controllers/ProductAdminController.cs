using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RogueFit.Data;

namespace RogueFit.Controllers
{
    public class ProductAdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ProductAdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var products = await dbContext.Products
                .Include(p => p.Tag)
                .ThenInclude(t => t.SubCategory)
                .ThenInclude(sc => sc.Category)
                .ToListAsync();

            return View(products);
        }
    }
}
