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
            vm.Categories = await dbContext.Categories
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToListAsync();

            if (vm.SelectedCategoryId.HasValue)
            {
                vm.SubCategories = await dbContext.SubCategories
                    .Where(sc => sc.CategoryId == vm.SelectedCategoryId.Value)
                    .Select(sc => new SelectListItem(sc.Name, sc.Id.ToString()))
                    .ToListAsync();
            }

            if (vm.SelectedSubCategoryId.HasValue)
            {
                vm.Tags = await dbContext.Tags
                    .Where(t => t.SubCategoryId == vm.SelectedSubCategoryId.Value)
                    .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
                    .ToListAsync();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            int categoryId;
            if (vm.SelectedCategoryId.HasValue)
            {
                categoryId = vm.SelectedCategoryId.Value;
            }
            else if (!string.IsNullOrWhiteSpace(vm.NewCategoryName))
            {
                var cat = new Category { Name = vm.NewCategoryName.Trim() };
                dbContext.Categories.Add(cat);
                await dbContext.SaveChangesAsync();
                categoryId = cat.Id;
            }
            else
            {
                ModelState.AddModelError(nameof(vm.SelectedCategoryId), "Please select or enter a Category");
                return View(vm);
            }

            int subCategoryId = -1;
            if (vm.SelectedSubCategoryId.HasValue)
            {
                subCategoryId = vm.SelectedSubCategoryId.Value;
            }
            else if (!string.IsNullOrWhiteSpace(vm.NewSubCategoryName))
            {
                var subCat = new SubCategory 
                { 
                    Name = vm.NewSubCategoryName.Trim(),
                    CategoryId = categoryId
                };
                dbContext.SubCategories.Add(subCat);
                await dbContext.SaveChangesAsync();
                categoryId = subCat.Id;
            }
            else
            {
                ModelState.AddModelError(nameof(vm.SelectedCategoryId), "Please select or enter a Sub Category");
                return View(vm);
            }

            int tagId;
            if (vm.SelectedTagId.HasValue)
            {
                tagId = vm.SelectedTagId.Value;
            }
            else if (!string.IsNullOrWhiteSpace(vm.NewTagName))
            {
                var tag = new Tag 
                { 
                    Name = vm.NewTagName.Trim(),
                    SubCategoryId = subCategoryId
                };
                dbContext.Tags.Add(tag);
                await dbContext.SaveChangesAsync();
                tagId = tag.Id;
            }
            else
            {
                ModelState.AddModelError(nameof(vm.SelectedTagId), "Please select or enter a Tag");
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
    }
}
