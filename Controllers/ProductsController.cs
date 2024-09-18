using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcPhone.Data;
using MvcPhone.Models;

namespace MvcPhone.Controllers
{
    [Authorize(Roles = "admin,employee")]

    public class ProductsController : Controller
    {
        private readonly PhoneDbContext _context;

        public ProductsController(PhoneDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString, int productGenre = 0)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'MvcPhoneContext.Phone'  is null.");
            }

            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Categories
                                            orderby m.Name
                                            select m.Name;
            var products = from m in _context.Product
                           select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (productGenre > 0)
            {
                products = products.Where(x => x.CategoryId == productGenre);
            }

            var productGenreVM = new ProductGenreViewModel
            {
                Genres = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name"),
                Products = await products.ToListAsync()
            };

            return View(productGenreVM);
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {

            var ProductEditView = new ProductEditView
            {
                Genres = await _context.Categories.ToListAsync(),
            };
            return View(ProductEditView);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên sản phẩm đã tồn tại chưa
                bool isDuplicateName = await _context.Product
                    .AnyAsync(p => p.Name.ToUpper() == product.Name.ToUpper());

                if (isDuplicateName)
                {
                    // Thêm lỗi vào ModelState và trả về form với thông báo
                    ModelState.AddModelError("Name", "Product name already exists. Please choose a different name.");

                    // Load lại danh sách categories cho dropdown nếu cần
                    var productEditView = new ProductEditView
                    {
                        Genres = await _context.Categories.ToListAsync(),
                        Product = product // giữ lại dữ liệu nhập của người dùng
                    };

                    return View(productEditView);
                }

                // Thêm sản phẩm mới nếu không có lỗi trùng lặp
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, trả về form với dữ liệu hiện tại
            var productEditViewWithError = new ProductEditView
            {
                Genres = await _context.Categories.ToListAsync(),
                Product = product
            };

            return View(productEditViewWithError);
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ReleaseDate,Genre,Price,Amount,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra tên sản phẩm có trùng không
                var existingProduct = await _context.Product
                    .FirstOrDefaultAsync(p => p.Name == product.Name && p.Id != product.Id);
                var ProductOld = await _context.Product.AsNoTracking().FirstOrDefaultAsync(p => p.Id == product.Id);
                if (ProductOld.Name != product.Name)
                {
                    if (existingProduct != null)
                    {
                         var productEditViewWithError = new ProductEditView
            {
                Genres = await _context.Categories.ToListAsync(),
                Product = product
            };

                        // Thông báo lỗi nếu tên sản phẩm đã tồn tại
                        ModelState.AddModelError("Name", "Product name already exists. Please choose a different name.");
                        return View(product);
                    }
                }


                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _context.Product.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            var productEditViewWithError = new ProductEditView
            {
                Genres = await _context.Categories.ToListAsync(),
                Product = Product
            };

            return View(productEditViewWithError);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
