using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCafe.Data;
using QLCafe.Models;

namespace QLCafe.Controllers.Admin
{
    public class BillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bills = _context.Bills.Include(b => b.Order);
            return View(await bills.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var bill = await _context.Bills
                .Include(b => b.Order)
                .FirstOrDefaultAsync(m => m.BillId == id);

            if (bill == null) return NotFound();

            return View(bill);
        }

        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(
                _context.Orders
                    .Where(o => !_context.Bills.Any(b => b.OrderId == o.OrderId)),
                "OrderId", "OrderId"
            );
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillId,OrderId,CreatedAt,TotalAmout,PaymentMethod")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // In lỗi validation ra console
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"[ModelState Error] {entry.Key}: {error.ErrorMessage}");
                }
            }

            ViewData["OrderId"] = new SelectList(
                _context.Orders
                    .Where(o => !_context.Bills.Any(b => b.OrderId == o.OrderId)),
                "OrderId", "OrderId", bill.OrderId
            );
            return View(bill);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var bill = await _context.Bills.FindAsync(id);
            if (bill == null) return NotFound();

            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", bill.OrderId);
            return View(bill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillId,OrderId,CreatedAt,TotalAmout,PaymentMethod")] Bill bill)
        {
            if (id != bill.BillId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.BillId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", bill.OrderId);
            return View(bill);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var bill = await _context.Bills
                .Include(b => b.Order)
                .FirstOrDefaultAsync(m => m.BillId == id);

            if (bill == null) return NotFound();

            return View(bill);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill != null)
            {
                _context.Bills.Remove(bill);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.BillId == id);
        }
    }
}
