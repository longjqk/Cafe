using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCafe.Data;
using QLCafe.Models;

namespace QLCafe.Controllers.Admin
{
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/OrderDetails
        public async Task<IActionResult> Index()
        {
            var data = _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Drink);
            return View(await data.ToListAsync());
        }

        // GET: Admin/OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["DrinkId"] = new SelectList(_context.Drinks, "DrinkId", "Name");
            return View();
        }

        // POST: Admin/OrderDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetail orderDetail)
        {
            if (orderDetail.Quantity < 0)
            {
                ModelState.AddModelError("Quantity", "Số lượng không được âm.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["DrinkId"] = new SelectList(_context.Drinks, "DrinkId", "Name", orderDetail.DrinkId);
            return View(orderDetail);
        }

        // GET: Admin/OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null) return NotFound();

            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["DrinkId"] = new SelectList(_context.Drinks, "DrinkId", "Name", orderDetail.DrinkId);
            return View(orderDetail);
        }

        // POST: Admin/OrderDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailId) return NotFound();

            if (orderDetail.Quantity < 0)
            {
                ModelState.AddModelError("Quantity", "Số lượng không được âm.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.OrderDetails.Any(e => e.OrderDetailId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["DrinkId"] = new SelectList(_context.Drinks, "DrinkId", "Name", orderDetail.DrinkId);
            return View(orderDetail);
        }

        // GET: Admin/OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var orderDetail = await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Drink)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderDetail == null) return NotFound();

            return View(orderDetail);
        }

        // POST: Admin/OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
