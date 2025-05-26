using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCafe.Data;
using QLCafe.Models;

namespace QLCafe.Controllers.Customer
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Drink)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            return View(cart);
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int drinkId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = user.Id,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.DrinkId == drinkId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    DrinkId = drinkId,
                    Quantity = quantity
                });
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Đã thêm vào giỏ hàng!";
            return RedirectToAction("Details", "Drinks", new { id = drinkId });
        }

        // Cập nhật số lượng
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null && quantity > 0)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}
