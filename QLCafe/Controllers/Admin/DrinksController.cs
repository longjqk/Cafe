using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCafe.Data;
using QLCafe.Models;

namespace QLCafe.Controllers.Admin
{
    public class DrinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHost;
        public DrinksController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        // GET: Drinks
        public async Task<IActionResult> Index(string searchString)
        {
            
            var drinks = _context.Drinks.Include(d => d.Category).AsQueryable(); ;

            if (!string.IsNullOrEmpty(searchString))
            {
                drinks = drinks.Where(d => d.DrinkName.Contains(searchString));
            }
            return View(await drinks.ToListAsync());
        }

        // GET: Drinks/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Drinks/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Drinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(IFormFile file, [Bind("DrinkId,DrinkName,Price,Des,ImgUrl,CategoryId")] Drink drink)
        {
            if (file == null)
            {
                ModelState.AddModelError("file", "Vui lòng chọn ảnh");
            }
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    string uploadFolder = Path.Combine(_webHost.WebRootPath, "images");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);

                    }
                    string fileName = Path.GetFileName(file.FileName);
                    string fileSavePath = Path.Combine(uploadFolder, fileName);

                    using(FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                    {
                         await file.CopyToAsync(stream);
                    }

                    drink.ImgUrl = "/images/" + fileName;
                }
                else
                {
                    ModelState.AddModelError("file", "Vui lòng chọn ảnh.");
                    ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", drink.CategoryId);
                    return View(drink);
                }


                drink.Rating = 0;
                _context.Add(drink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage); // Hoặc log ra file/log system
                    }
                }
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", drink.CategoryId);
            return View(drink);
        }

        // GET: Drinks/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", drink.CategoryId);
            return View(drink);
        }

        // POST: Drinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("DrinkId,Rating,DrinkName,Price,Des,ImgUrl,CategoryId")] Drink drink)
        {
            if (id != drink.DrinkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrinkExists(drink.DrinkId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", drink.CategoryId);
            return View(drink);
        }

        // GET: Drinks/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
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

            return View(drink);
        }

        // POST: Drinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drink = await _context.Drinks.FindAsync(id);
            if (drink != null)
            {
                _context.Drinks.Remove(drink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrinkExists(int id)
        {
            return _context.Drinks.Any(e => e.DrinkId == id);
        }
    }
}
