using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLCafe.Data;
using QLCafe.Models;

namespace QLCafe.Controllers.Customer
{
    public class DrinksCustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DrinksCustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DrinksCustomer
        public async Task<IActionResult> IndexDrinkCustomer(string sortOrder)
        {
           
            var drinks = _context.Drinks
                .Include(d => d.Category)
                .AsQueryable();

            switch (sortOrder)
            {
                case "PriceAsc":
                    drinks = drinks.OrderBy(d => d.Price);
                    break;
                case "PriceDesc":
                    drinks = drinks.OrderByDescending(d => d.Price);
                    break;
                default:
                    drinks = drinks.OrderBy(d => d.DrinkName);
                    break;
            }

            return View(await drinks.ToListAsync());
        }

        // GET: DrinksCustomer/Details/5
        public async Task<IActionResult> DetailsDrinkCustomer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drinks
                .Include(d => d.Category)
                .FirstOrDefaultAsync(m => m.DrinkId == id);
            if (drink == null)
            {
                return NotFound();
            }
            var toppings = await _context.Toppings.ToListAsync(); // Lấy tất cả topping

            ViewBag.Toppings = toppings; // Gửi xuống View

            return View(drink);
        }

    }
}
