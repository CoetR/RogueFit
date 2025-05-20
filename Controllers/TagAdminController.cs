using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RogueFit.Data;
using RogueFit.Models;
using RogueFit.ViewModels;

namespace RogueFit.Controllers
{
    public class TagAdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public TagAdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await dbContext.Tags.Include(t => t.SubCategory).ToListAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new TagViewModel
            {
                SubCategories = await dbContext.SubCategories
                    .Select(sc => new SelectListItem(sc.Name, sc.Id.ToString())).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.SubCategories = await dbContext.SubCategories
                    .Select(sc => new SelectListItem(sc.Name, sc.Id.ToString())).ToListAsync();
                return View(vm);
            }

            var tag = new Tag
            {
                Name = vm.Name,
                SubCategoryId = vm.SelectedSubCategoryId
            };

            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tag = await dbContext.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            var vm = new TagViewModel
            {
                Name = tag.Name,
                SelectedSubCategoryId = tag.SubCategoryId,
                SubCategories = await dbContext.SubCategories
                    .Select(sc => new SelectListItem(sc.Name, sc.Id.ToString())).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TagViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.SubCategories = await dbContext.SubCategories
                    .Select(sc => new SelectListItem(sc.Name, sc.Id.ToString())).ToListAsync();
            }

            var tag = await dbContext.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            tag.Name = vm.Name;
            tag.SubCategoryId = vm.SelectedSubCategoryId;

            dbContext.Tags.Update(tag);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await dbContext.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            dbContext.Tags.Remove(tag);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
