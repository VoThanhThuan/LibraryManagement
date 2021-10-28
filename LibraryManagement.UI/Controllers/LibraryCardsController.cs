using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Library.Data;
using Library.Library.Entities;
using LibraryManagement.UI.Models.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.UI.Controllers
{
    public class LibraryCardsController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly IStorageService _storage;
        private readonly IConfiguration _config; //lấy config từ appsetting.config

        public LibraryCardsController(LibraryDbContext context, IStorageService storage, IConfiguration config)
        {
            _context = context;
            _storage = storage;
            _config = config;
        }

        // GET: LibraryCards
        public async Task<IActionResult> Index()
        {
            var cards = await _context.LibraryCards.ToListAsync();
            var host = _config.GetSection("BaseAddress").Value;
            foreach (var book in cards)
                book.Image = $"{host}/{book.Image}";

            return View(cards);
        }

        // GET: LibraryCards/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryCard = await _context.LibraryCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryCard == null)
            {
                return NotFound();
            }

            var host = _config.GetSection("BaseAddress").Value;

            libraryCard.Image = $"{host}/{libraryCard.Image}";

            return View(libraryCard);
        }

        // GET: LibraryCards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LibraryCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MSSV,Class,PhoneNumber,Karma,IsLock,Rank,Exp,ExpLevelUp")] LibraryCard libraryCard, IFormFile Image)
        {
            if (!ModelState.IsValid) return View(libraryCard);
            libraryCard.Id = Guid.NewGuid();
            libraryCard.Image = await _storage.SaveFileAsync(Image, "libraryCard");
            _context.Add(libraryCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: LibraryCards/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryCard = await _context.LibraryCards.FindAsync(id);
            if (libraryCard == null)
            {
                return NotFound();
            }
            return View(libraryCard);
        }

        // POST: LibraryCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,MSSV,Class,PhoneNumber,Karma,IsLock,Rank,Exp,ExpLevelUp")] LibraryCard libraryCard, IFormFile Image)
        {
            if (id != libraryCard.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(libraryCard);
            try
            {
                libraryCard.Image = await _storage.SaveFileAsync(Image, "libraryCard");
                _context.Update(libraryCard);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryCardExists(libraryCard.Id))
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

        // GET: LibraryCards/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libraryCard = await _context.LibraryCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryCard == null)
            {
                return NotFound();
            }

            return View(libraryCard);
        }

        // POST: LibraryCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var libraryCard = await _context.LibraryCards.FindAsync(id);
            _context.LibraryCards.Remove(libraryCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryCardExists(Guid id)
        {
            return _context.LibraryCards.Any(e => e.Id == id);
        }
    }
}
