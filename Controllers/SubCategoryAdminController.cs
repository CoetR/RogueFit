using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RogueFit.Data;
using RogueFit.Models;

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

        public async Task<IActionResult> Create(SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await dbContext.Categories.ToListAsync(), "Id", "Name");
                return View(subCategory);
            }

            dbContext.SubCategories.Add(subCategory);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var subCategory = await dbContext.SubCategories.FindAsync(id);
            if (subCategory == null) return NotFound();

            ViewBag.Categories = new SelectList(await dbContext.Categories.ToListAsync(), "Id", "Name");
            return View(subCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await dbContext.Categories.ToListAsync(), "Id", "Name");
                return View(subCategory);
            }

            dbContext.SubCategories.Update(subCategory);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var subCategory = await dbContext.SubCategories.Include(sc => sc.Category)
                .FirstOrDefaultAsync(sc => sc.Id == id);
            if (subCategory == null) return NotFound();
            return View(subCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await dbContext.SubCategories.FindAsync(id);
            if (subCategory != null)
            {
                dbContext.SubCategories.Remove(subCategory);
                await dbContext.SaveChangesAsync(); 
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
