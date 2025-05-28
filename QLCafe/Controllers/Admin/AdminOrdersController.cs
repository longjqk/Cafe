using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCafe.Data;
using QLCafe.Models;

namespace QLCafe.Controllers
{
    public class AdminOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminOrders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Bill)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Drink)
                .ToListAsync();

            return View(orders);
        }

        // GET: AdminOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Bill)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Drink)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
