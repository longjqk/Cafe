//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using NuGet.Protocol.Core.Types;
//using QLCafe.Data;
//using QLCafe.Models;
//using System;

//namespace QLCafe.Controllers
//{
//    public class ProductController : Controller
//    {

//        private readonly ApplicationDbContext _context;

//        public ProductController(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        GET: Product
//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.Products.ToListAsync());
//        }

//        GET: Product/Details/5
//        public async Task<IActionResult> Details(int id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Products
//                .FirstOrDefaultAsync(m => m.Id == id);

//            if (product == null)
//                return NotFound();

//            return View(product);
//        }

//        GET: Product/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        POST: Product/Create
//       [HttpPost]
//       [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Product product)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(product);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(product);
//        }

//        GET: Product/Edit/5
//        public async Task<IActionResult> Edit(int id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Products
//                .FirstOrDefaultAsync(m => m.Id == id);

//            if (product == null)
//                return NotFound();

//            return View(product);
//        }

//        POST: Product/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, Product product)
//        {
//            if (id != product.Id)
//            {
//                return BadRequest();
//            }
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(product);
//                    await _context.SaveChangesAsync();
//                    return RedirectToAction(nameof(Index));
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!_context.Products.Any(e => e.Id == id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//            }
//            return View(product);
//        }

//        GET: Product/Delete/5
//        public async Task<IActionResult> Delete(int id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Products
//                .FirstOrDefaultAsync(m => m.Id == id);

//            if (product == null)
//                return NotFound();

//            return View(product);
//        }

//        POST: Product/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var product = await _context.Products.FindAsync(id);

//            if (product != null)
//            {
//                _context.Products.Remove(product);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
