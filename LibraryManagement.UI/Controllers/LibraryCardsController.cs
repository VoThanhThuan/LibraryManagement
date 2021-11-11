using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Library.Data;
using Library.Library.Entities;
using LibraryManagement.UI.Models.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.UI.Controllers {
    [Authorize]
    public class LibraryCardsController : Controller {
        private readonly LibraryDbContext _context;
        private readonly IStorageService _storage;
        private readonly IConfiguration _config; //lấy config từ appsetting.config

        public LibraryCardsController(LibraryDbContext context, IStorageService storage, IConfiguration config) {
            _context = context;
            _storage = storage;
            _config = config;
        }

        // GET: LibraryCards
        public async Task<IActionResult> Index() {
            var cards = await _context.LibraryCards.ToListAsync();

            return View(cards);
        }

        // GET: LibraryCards/Search/content
        [HttpGet]
        public async Task<IActionResult> Search(string content, int take = 10) {
            if (take > 40)
                take = 40;
            if (string.IsNullOrEmpty(content))
                return RedirectToAction("Index");
            content = content.ToLower();
            if (string.IsNullOrEmpty(content)) return NoContent();
            var LibraryCard = await _context.LibraryCards
                                    .Where(x => x.MSSV.ToLower().Contains(content) || x.Name.ToLower().Contains(content))
                                    .Take(take)
                                    .ToListAsync();
            ViewData["Content Search"] = content;
            return View("Index", LibraryCard);
        }

        // GET: LibraryCards/Details/5
        public async Task<IActionResult> Details(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var libraryCard = await _context.LibraryCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryCard == null) {
                return NotFound();
            }

            return View(libraryCard);
        }

        // GET: LibraryCards/Create
        public IActionResult Create() {
            return View();
        }

        // POST: LibraryCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MSSV,Name,Class,PhoneNumber,Karma,IsLock,Rank,Exp,ExpLevelUp,StatusCard")] LibraryCard libraryCard, IFormFile Image) {
            if (!ModelState.IsValid) return View(libraryCard);
            libraryCard.Id = Guid.NewGuid();
            libraryCard.Image = await _storage.SaveFileAsync(Image, "libraryCard");
            _context.Add(libraryCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: LibraryCards/Edit/5
        public async Task<IActionResult> Edit(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var libraryCard = await _context.LibraryCards.FindAsync(id);
            if (libraryCard == null) {
                return NotFound();
            }
            return View(libraryCard);
        }

        // POST: LibraryCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,MSSV,Name,Class,PhoneNumber,Karma,IsLock,Rank,Exp,ExpLevelUp,StatusCard")] LibraryCard libraryCard, IFormFile Image) {
            if (id != libraryCard.Id) {
                return NotFound();
            }


            if (!ModelState.IsValid) return View(libraryCard);

            var libCard = await _context.LibraryCards.FindAsync(id);

            libCard.MSSV = libraryCard.MSSV;
            libCard.Name = libraryCard.Name;
            libCard.Class = libraryCard.Class;
            libCard.PhoneNumber = libraryCard.PhoneNumber;
            libCard.Karma = libraryCard.Karma;
            libCard.IsLock = libraryCard.IsLock;
            libCard.Rank = libraryCard.Rank;
            libCard.Exp = libraryCard.Exp;
            libCard.ExpLevelUp = libraryCard.ExpLevelUp;
            libCard.StatusCard = libraryCard.StatusCard;

            if (Image is not null) {
                await _storage.DeleteFileAsync(libCard.Image);
                libCard.Image = await _storage.SaveFileAsync(Image, "libraryCard");
            }

            _context.LibraryCards.Update(libCard);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: LibraryCards/Delete/5
        public async Task<IActionResult> Delete(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var libraryCard = await _context.LibraryCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryCard == null) {
                return NotFound();
            }

            return View(libraryCard);
        }

        // POST: LibraryCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id) {
            var libraryCard = await _context.LibraryCards.FindAsync(id);
            _context.LibraryCards.Remove(libraryCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryCardExists(Guid id) {
            return _context.LibraryCards.Any(e => e.Id == id);
        }
    }
}
