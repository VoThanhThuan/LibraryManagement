﻿using System;
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
using Library.Library.Entities.ViewModels;
using LibraryManagement.UI.Services;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.UI.Controllers
{
    [Route("[controller]")]
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

        [HttpGet("Details")]
        // GET: Borrows/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
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
        // GET: Borrows/Create/xxx-xxx-xxx
        [HttpGet("Create/{idCard}")]
        public async Task<IActionResult> Create(Guid? idCard = null)
        {
            ViewData["Id-User"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Name-User"] = (User.FindFirstValue(ClaimTypes.Name));
            ViewData["cardSelect"] = Guid.Empty;
            var borrow = new Borrow();

            if (idCard is not null)
            {
                var cardSelect = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == idCard);
                if (cardSelect is not null)
                {
                    ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "MSSV", cardSelect.Id);
                    borrow.IdCard = cardSelect.Id;
                }

            }
            else
            {
                ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "MSSV");
            }


            var books = await _book.GetBooks();

            return View((borrow, books));
        }

        // POST: Borrows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateBorrow,Note,IdUser,UserName,IdCard")] Borrow borrow, List<string> idBooks)
        {
            ViewData["Id-User"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Name-User"] = (User.FindFirstValue(ClaimTypes.Name));
            ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "MSSV");

            var isSuccess = false;

            if (ModelState.IsValid)
            {
                var result = await _borrow.PostBorrow(borrow, idBooks);

                isSuccess = result.isSuccess;

            }

            ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "Id", borrow.IdCard);
            var books = await _book.GetBooks();
            if (isSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View((borrow, books));
            }
        }

        [HttpGet("ReturnBook")]
        public async Task<IActionResult> ReturnBook(Guid idCard , Guid idBorrow)
        {
            var cardAndBook = new ReturnBookVM();

            var card = _context.LibraryCards.Find(idCard);

            if (card is null)
                return RedirectToAction(nameof(Index));

            ViewBag.LibraryCard = card;
            if (idCard != Guid.Empty)
            {
                cardAndBook = await _borrow.GetBorrowWithCard(idCard);
                if(cardAndBook is null)
                    return RedirectToAction(nameof(Index));

                cardAndBook.IdCard = idCard;

                return View(cardAndBook);
            }else if (idBorrow != Guid.Empty)
            {
                cardAndBook = await _borrow.GetBorrow(idBorrow);
                return View(cardAndBook);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("ReturnBook")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(ReturnBookRequest request)
        {
            var cardAndBook = new ReturnBookVM();
            ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "Id", request.IdCard);

            if (request.IdCard != Guid.Empty)
            {
                var resultGet = await _borrow.GetBorrowWithCard(request.IdCard);

                resultGet.IdCard = request.IdCard;
                cardAndBook = resultGet;
            }
            else if (request.IdBorrow != Guid.Empty)
            {
                cardAndBook = await _borrow.GetBorrow(request.IdBorrow);
            }

            var result = await _borrow.ReturnBook(request);
            if (result == false)
            {
                return View(cardAndBook);
            }

            return Redirect($"/Borrow/ReturnBook?idCard={request.IdCard}");
        }

        [HttpGet("Edit")]
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

        [HttpGet("Delete")]
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
