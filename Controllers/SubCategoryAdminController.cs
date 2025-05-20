using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RogueFit.Data;
using RogueFit.Models;
using RogueFit.ViewModels;

namespace RogueFit.Controllers
{
    public class SubCategoryAdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public SubCategoryAdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var subCategories = await dbContext.SubCategories.Include(sc => sc.Category)
                .ToListAsync();
            return View(subCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new SubCategoryViewModel
            {
                Categories = await dbContext.Categories
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubCategoryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await dbContext.Categories
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToListAsync();
                return View(vm);
            }

            var subCategory = new SubCategory
            {
                Name = vm.Name,
                CategoryId = vm.SelectedCategoryId
            };

            dbContext.SubCategories.Add(subCategory);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var subCategory = await dbContext.SubCategories.FindAsync(id);
            if (subCategory == null) return NotFound();

            var vm = new SubCategoryViewModel
            {
                Name = subCategory.Name,
                SelectedCategoryId = subCategory.CategoryId,
                Categories = await dbContext.Categories
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.Categories = await dbContext.Categories
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToListAsync();
                return View(vm);
            }

            var subCategory = await dbContext.SubCategories.FindAsync(id);
            if (subCategory == null) return NotFound();

            subCategory.Name = vm.Name;
            subCategory.CategoryId = vm.SelectedCategoryId;

            dbContext.SubCategories.Update(subCategory);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var subCategory = await dbContext.SubCategories.FindAsync(id);
            if (subCategory == null) return NotFound();
            
            dbContext.SubCategories.Remove(subCategory);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
