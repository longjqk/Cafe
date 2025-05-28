using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCafe.Data;
using QLCafe.Models;

namespace QLCafe.Controllers
{
    public class ToppingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToppingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Topping
        public async Task<IActionResult> Index()
        {
            return View(await _context.Toppings.ToListAsync());
        }

        // GET: Topping/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Topping/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ToppingId,ToppingName,Price")] Topping topping)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topping);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topping);
        }

        // GET: Topping/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var topping = await _context.Toppings.FindAsync(id);
            if (topping == null) return NotFound();

            return View(topping);
        }

        // POST: Topping/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ToppingId,ToppingName,Price")] Topping topping)
        {
            if (id != topping.ToppingId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Toppings.Any(e => e.ToppingId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(topping);
        }

        // GET: Topping/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var topping = await _context.Toppings.FirstOrDefaultAsync(m => m.ToppingId == id);
            if (topping == null) return NotFound();

            return View(topping);
        }

        // POST: Topping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topping = await _context.Toppings.FindAsync(id);
            if (topping != null)
            {
                _context.Toppings.Remove(topping);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
