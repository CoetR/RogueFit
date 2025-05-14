using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RogueFit.Data;
using RogueFit.Models;
using RogueFit.ViewModels;

namespace RogueFit.Controllers
{
    public class ProductAdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment env;

        public ProductAdminController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            this.dbContext = dbContext;
            this.env = env;
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

        public async Task<IActionResult> Create()
        {
            var vm = new ProductCreateViewModel
            {
                Categories = await dbContext.Categories
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategories(vm);
                return View(vm);
            }

            if (!vm.SelectedCategoryId.HasValue)
            {
                ModelState.AddModelError(nameof(vm.SelectedCategoryId), "Please select a Category");
                await PopulateCategories(vm);
                return View(vm);
            }

            int categoryId = vm.SelectedCategoryId.Value;

            int subCategoryId;
            if (vm.SelectedSubCategoryId.HasValue)
            {
                subCategoryId = vm.SelectedSubCategoryId.Value;
            }
            else
            {
                ModelState.AddModelError(nameof(vm.SelectedSubCategoryId), "Please select a Sub Category");
                await PopulateCategories(vm);
                return View(vm);
            }

            int tagId;
            if (vm.SelectedTagId.HasValue)
            {
                tagId = vm.SelectedTagId.Value;
            }
            else
            {
                ModelState.AddModelError(nameof(vm.SelectedTagId), "Please select Tag");
                await PopulateCategories(vm);
                return View(vm);
            }

            string imageFileName = null;
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                var uploads = Path.Combine(env.WebRootPath, "images");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                imageFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(vm.ImageFile.FileName);
                var filePath = Path.Combine(uploads, imageFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await vm.ImageFile.CopyToAsync(stream);
            }

            var product = new Product
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                TagId = tagId,
                Image = imageFileName
            };
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        private async Task PopulateCategories(ProductCreateViewModel vm)
        {
            vm.Categories = await dbContext.Categories
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToListAsync();

            vm.SubCategories = await dbContext.SubCategories
                .Where(sc => sc.CategoryId == vm.SelectedCategoryId)
                .Select(sc => new SelectListItem(sc.Name, sc.Id.ToString()))
                .ToListAsync();

            vm.Tags = await dbContext.Tags
                .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
                .ToListAsync();
        }
    }
}
