using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.Requests;
using LibraryManagement.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.UI.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly BookService _book;

        public BooksController(LibraryDbContext context, BookService book)
        {
            _context = context;
            _book = book;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = await _book.GetBooks();

            return View(books);
        }

        // GET: Books/Search/content
        [AllowAnonymous]
        public async Task<IActionResult> Search(string content)
        {
            if (string.IsNullOrEmpty(content)) return NoContent();
            var books = await _book.SearchBook(content);
            return Ok(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _book.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["IdLibraryCode"] = new SelectList(_context.LibraryCodes, "Id", "Id");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookRequest request)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            var book = await _book.PostBook(request);
                
            ViewData["IdLibraryCode"] = new SelectList(_context.LibraryCodes, "Id", "Id", book.IdLibraryCode);
            return View(book.ToRequest());
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["IdLibraryCode"] = new SelectList(_context.LibraryCodes, "Id", "Id", book.IdLibraryCode);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, BookRequest request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            var book = await _book.PutBook(request);

            ViewData["IdLibraryCode"] = new SelectList(_context.LibraryCodes, "Id", "Id", book.IdLibraryCode);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.LibraryCode)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _book.DeleteBook(id);
            if (result is not 200)
                return Conflict();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(string id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
