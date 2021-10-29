using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.Requests;
using LibraryManagement.UI.Services;

namespace LibraryManagement.UI.Controllers
{
    public class BorrowsController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly BorrowService _borrow;
        private readonly BookService _book;

        public BorrowsController(LibraryDbContext context, BorrowService borrow, BookService book)
        {
            _context = context;
            _borrow = borrow;
            _book = book;
        }

        // GET: Borrows
        public async Task<IActionResult> Index()
        {
            //var libraryDbContext = _context.Borrows.Include(b => b.LibraryCard);
            var ListCardAndBorrow = await _borrow.GetBorrows();
            return View(ListCardAndBorrow);
        }

        // GET: Borrows/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows
                .Include(b => b.LibraryCard)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }

        // GET: Borrows/Create
        public async Task<IActionResult> Create()
        {
            var books = await _book.GetBooks();
            ViewData["Id-User"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Name-User"] = (User.FindFirstValue(ClaimTypes.Name));
            ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "MSSV");
            var borrow = new Borrow();
            return View((borrow, books));
        }

        // POST: Borrows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateBorrow,Note,IdUser,UserName,IdCard")] Borrow borrow, List<string> idBooks)
        {
            var books = await _book.GetBooks();
            ViewData["Id-User"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Name-User"] = (User.FindFirstValue(ClaimTypes.Name));
            ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "MSSV");

            if (ModelState.IsValid)
            {
                var result = await _borrow.PostBorrow(borrow, idBooks);

                if (!result.isSuccess) return View((borrow, books));

                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "Id", borrow.IdCard);
            return View((borrow, books));
        }

        // GET: Borrows/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return NotFound();
            }
            ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "Id", borrow.IdCard);
            return View(borrow);
        }

        // POST: Borrows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DateBorrow,Note,IdUser,UserName,IdCard")] Borrow borrow)
        {
            if (id != borrow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowExists(borrow.Id))
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
            ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "Id", borrow.IdCard);
            return View(borrow);
        }

        // GET: Borrows/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows
                .Include(b => b.LibraryCard)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }

        // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var borrow = await _context.Borrows.FindAsync(id);
            _context.Borrows.Remove(borrow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowExists(Guid id)
        {
            return _context.Borrows.Any(e => e.Id == id);
        }
    }
}
