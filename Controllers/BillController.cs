using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Exceptions;
using Microsoft.EntityFrameworkCore;
using MvcPhone.Data;
using MvcPhone.Models;

namespace MvcPhone.Controllers
{
    [Authorize(Roles = "admin,employee")]

    public class BillController : Controller
    {
        private readonly PhoneDbContext _context;

        public BillController(PhoneDbContext context)
        {
            _context = context;
        }

        // GET: Bill
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bill.Include(p => p.Product).ToListAsync());
        }

        // GET: Bill/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bill
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // GET: Bill/Create
        public IActionResult Create(int? Id)
        {
            if (Id == null){
                return NotFound();
            }
            var product = _context.Product.FirstOrDefault(m => m.Id == Id);
            if (product == null){
                return NotFound();
            }
            var bill = new Bill{
                Product = product,
                DateTime = DateTime.Now,
                ProductId = product.Id
            };
            return View(bill);
        }

        // POST: Bill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       public async Task<IActionResult> Create([Bind("ProductId,DateTime,Amount,Price")] Bill bill)
{
    if (ModelState.IsValid)
    {
        var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == bill.ProductId);
        
        if (product != null)
        {
            if (product.Amount >= bill.Amount)
            {
                _context.Add(bill);
                product.Amount -= bill.Amount;
                _context.Product.Update(product);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Thêm thông báo lỗi vào ModelState nếu số lượng vượt quá tồn kho
                ModelState.AddModelError(string.Empty, "Amount exceeds available stock.");
            }
        }
    }
    
    // Nếu có lỗi hoặc số lượng vượt quá tồn kho, trả về view với thông tin bill và sản phẩm
    bill.Product = await _context.Product.FirstOrDefaultAsync(p => p.Id == bill.ProductId);
    return View(bill);
}

        

        // GET: Bill/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bill.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            return View(bill);
        }

        // POST: Bill/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,DateTime,Amount,Price")] Bill bill)
        {
            if (id != bill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.Id))
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
            return View(bill);
        }

        // GET: Bill/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bill
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // POST: Bill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bill = await _context.Bill.FindAsync(id);
            if (bill != null)
            {
                _context.Bill.Remove(bill);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillExists(int id)
        {
            return _context.Bill.Any(e => e.Id == id);
        }
    }
}
