using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            if (user == null) return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Drink)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.CartItemToppings)
                        .ThenInclude(cit => cit.Topping)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null)
            {
                cart = new Cart { CartItems = new List<CartItem>() };
            }
            //decimal cartTotal = 0;

            
            foreach (var item in cart.CartItems)
            {
                // Lấy tổng giá topping cho item này
                decimal totalToppingPrice = item.CartItemToppings.Sum(ct => (decimal)ct.Topping.Price);

                // Cập nhật topping price vào CartItem nếu cần
                item.ToppingPrice = totalToppingPrice;

                // Tính tổng tiền 1 item (đồ uống + topping) * số lượng
                //decimal itemTotalPrice = (item.Drink.Price + totalToppingPrice) * item.Quantity;

                //cartTotal += itemTotalPrice;
                var toppingNames = item.CartItemToppings.Select(t => t.Topping.ToppingName);
                item.ToppingDescription = string.Join(", ", toppingNames);
            }

            return View(cart);
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var cart = await _context.Carts
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CartId == id);

            if (cart == null)
                return NotFound();

            return View(cart);
        }

        // POST: Carts/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int drinkId, int quantity, [FromForm] List<int> toppingIds)
        {
            if (quantity < 1)
                quantity = 1;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.CartItemToppings)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = user.Id,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Lấy topping text để so sánh
            var toppingNames = toppingIds != null && toppingIds.Any()
                ? await _context.Toppings.Where(t => toppingIds.Contains(t.ToppingId)).Select(t => t.ToppingName).ToListAsync()
                : new List<string>();

            string toppingText = string.Join(", ", toppingNames);

            // Tìm CartItem đã tồn tại: cùng drink + cùng topping set (so sánh text)
            var existingItem = cart.CartItems.FirstOrDefault(ci =>
                ci.DrinkId == drinkId &&
                ci.ToppingDescription == toppingText);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    DrinkId = drinkId,
                    Quantity = quantity,
                    ToppingDescription = toppingText,
                    CartItemToppings = new List<CartItemTopping>()
                };

                if (toppingIds != null && toppingIds.Any())
                {
                    foreach (var toppingId in toppingIds)
                    {
                        var topping = await _context.Toppings.FindAsync(toppingId);
                        if (topping != null)
                        {
                            newItem.CartItemToppings.Add(new CartItemTopping
                            {
                                ToppingId = toppingId,
                                ToppingPrice = (decimal)topping.Price
                            });
                        }
                    }
                }

                cart.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Đã thêm vào giỏ hàng!";

            return RedirectToAction("Details", "Drinks", new { id = drinkId });
        }

        // Cập nhật số lượng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            if (quantity < 1)
                quantity = 1;

            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // Xóa item trong giỏ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.CartItemToppings)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

            if (cartItem != null)
            {
                // Xóa topping liên quan trước
                _context.CartItemToppings.RemoveRange(cartItem.CartItemToppings);
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(string selectedItemIds)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(selectedItemIds))
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một sản phẩm để mua.";
                return RedirectToAction("Index");
            }

            // Tách chuỗi id thành int
            var selectedIds = selectedItemIds.Split(',')
                .Select(idStr => {
                    bool parsed = int.TryParse(idStr, out int id);
                    return new { parsed, id };
                })
                .Where(x => x.parsed)
                .Select(x => x.id)
                .ToList();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.CartItemToppings)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null || cart.CartItems.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            // Tạo đơn hàng mới
            var order = new Order
            {
                UserId = user.Id,
                Status = "Đang xử lý",
                OrderDate = DateTime.Now,
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var item in cart.CartItems.Where(ci => selectedIds.Contains(ci.CartItemId)))
            {
                var orderDetail = new OrderDetail
                {
                    DrinkId = item.DrinkId,
                    Quantity = item.Quantity,
                    OrderDetailToppings = new List<OrderDetailTopping>()
                };
                // Thêm OrderDetailToppings tương ứng với CartItemToppings
                foreach (var cartItemTopping in item.CartItemToppings)
                {
                    var orderDetailTopping = new OrderDetailTopping
                    {
                        ToppingId = cartItemTopping.ToppingId,
                         
                        Price = (double)cartItemTopping.ToppingPrice
                    };

                    orderDetail.OrderDetailToppings.Add(orderDetailTopping);
                }

                order.OrderDetails.Add(orderDetail);
            }

            _context.Orders.Add(order);

            var itemsToRemove = cart.CartItems.Where(ci => selectedIds.Contains(ci.CartItemId)).ToList();
            // Xóa giỏ hàng sau khi đặt hàng
            _context.CartItemToppings.RemoveRange(itemsToRemove.SelectMany(ci => ci.CartItemToppings));
            _context.CartItems.RemoveRange(itemsToRemove);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "Orders", new { id = order.OrderId});
        }
    }


}
