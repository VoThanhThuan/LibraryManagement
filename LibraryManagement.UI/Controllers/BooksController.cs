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
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly BookService _book;
        private readonly GenreService _genre;

        public BooksController(LibraryDbContext context, BookService book, GenreService genre)
        {
            _context = context;
            _book = book;
            _genre = genre;
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
            if (string.IsNullOrEmpty(content)) return Redirect("Index");
            var books = await _book.SearchBook(content);
            ViewData["Content Search"] = content;

            return View("Index", books);
        }

        // GET: Books/Search/content
        [AllowAnonymous]
        public async Task<IActionResult> SearchApi(string content)
        {
            if (string.IsNullOrEmpty(content)) return NoContent();
            var books = await _book.SearchBook(content);
            return Ok(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) {
                TempData["error"] = "Không tìm thấy sách";
                return RedirectToAction(nameof(Index));
            }

            var book = await _book.GetBook(id);
            if (book == null) {
                TempData["error"] = "Không tìm thấy sách";
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["Genre"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["IdLibraryCode"] = new SelectList(_context.LibraryCodes, "Id", "Id");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookRequest request, List<int> idGenres)
        {
            ViewData["Genre"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["IdLibraryCode"] = new SelectList(_context.LibraryCodes, "Id", "Id");

            if (!ModelState.IsValid) return View(request);

            var book = await _book.PostBook(request);
            await _genre.PostBookInGenre(book.Id, idGenres);

            TempData["success"] = $"Thêm mới sách {request.Name} thành công";
            return View(book.ToRequest());
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) {
                TempData["error"] = "Không tìm thấy sách";
                return RedirectToAction(nameof(Index));
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null) {
                TempData["error"] = "Không tìm thấy sách";
                return RedirectToAction(nameof(Index));
            }

            var bookRequest = book.ToRequest();
            bookRequest.idGenres = await _genre.GetIdBookInGenre(book.Id);

            ViewData["Genre"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["IdLibraryCode"] = new SelectList(_context.LibraryCodes, "Id", "Id", book.IdLibraryCode);
            return View(bookRequest);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, BookRequest request)
        {
            if (id != request.Id) {
                return NotFound();
            }


            ModelState.Remove("Thumbnail");
            if (!ModelState.IsValid) return View(request);

            var book = await _book.PutBook(request);
            await _genre.PostBookInGenre(book.Id, request.idGenres);

            var bookRequest = book.ToRequest();
            bookRequest.idGenres = await _genre.GetIdBookInGenre(book.Id);

            ViewData["Genre"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["IdLibraryCode"] = new SelectList(_context.LibraryCodes, "Id", "Id", book.IdLibraryCode);
            TempData["success"] = $"Đã sửa {book.Name} thành công";
            return View(bookRequest);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) {
                TempData["error"] = "Không tìm thấy sách";
                return RedirectToAction(nameof(Index));
            }

            var book = await _context.Books
                .Include(b => b.LibraryCode)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null) {
                TempData["error"] = "Không tìm thấy sách";
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _book.DeleteBook(id);
            if (result is not 200) {
                TempData["error"] = $"Đã xóa sách có id là {id} thất bại";
                return RedirectToAction("Index");
            }
            TempData["success"] = $"Đã xóa sách có id là {id} thành công";
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(string id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
