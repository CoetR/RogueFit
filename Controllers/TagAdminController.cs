using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RogueFit.Data;
using RogueFit.Models;

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

        public async Task<IActionResult> Create()
        {
            ViewBag.SubCategories = new SelectList(await dbContext.SubCategories.Include(sc => sc.Category).ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubCategories = new SelectList(await dbContext.SubCategories.Include(sc => sc.Category).ToListAsync(), "Id", "Name");
                return View(tag);
            }

            dbContext.Tags.Add(tag);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await dbContext.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            ViewBag.SubCategories = new SelectList(await dbContext.SubCategories.Include(sc => sc.Category).ToListAsync(), "Id", "Name");
            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubCategories = new SelectList(await dbContext.SubCategories.Include(sc => sc.Category).ToListAsync(), "Id", "Name");
                return View(tag);
            }

            dbContext.Tags.Update(tag);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tag = await dbContext.Tags.Include(t => t.SubCategory).FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await dbContext.Tags.FindAsync(id);
            if (tag != null)
            {
                dbContext.Tags.Remove(tag);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
