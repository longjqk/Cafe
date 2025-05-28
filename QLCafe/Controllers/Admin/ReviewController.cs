using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCafe.Data;
using QLCafe.Models;

namespace QLCafe.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Drink);
            return View(await reviews.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Drink)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null) return NotFound();

            return View(review);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["DrinkId"] = new SelectList(_context.Drinks, "DrinkId", "DrinkName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewId,Rating,Comment,UserId,DrinkId")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", review.UserId);
            ViewData["DrinkId"] = new SelectList(_context.Drinks, "DrinkId", "DrinkName", review.DrinkId);
            return View(review);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", review.UserId);
            ViewData["DrinkId"] = new SelectList(_context.Drinks, "DrinkId", "DrinkName", review.DrinkId);
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,Rating,Comment,UserId,DrinkId")] Review review)
        {
            if (id != review.ReviewId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", review.UserId);
            ViewData["DrinkId"] = new SelectList(_context.Drinks, "DrinkId", "DrinkName", review.DrinkId);
            return View(review);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Drink)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null) return NotFound();

            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}
