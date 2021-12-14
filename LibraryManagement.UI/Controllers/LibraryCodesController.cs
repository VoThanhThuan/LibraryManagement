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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LibraryCodesController : Controller
    {
        private readonly LibraryDbContext _context;

        public LibraryCodesController(LibraryDbContext context)
        {
            _context = context;
        }


        // GET: LibraryCodes
        public async Task<IActionResult> Index()
        {
            return View(await _context.LibraryCodes.ToListAsync());
        }

        // GET: LibraryCodes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) {
                return NotFound();
            }

            var libraryCode = await _context.LibraryCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryCode == null) {
                return NotFound();
            }

            return View(libraryCode);
        }

        // GET: LibraryCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LibraryCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Abbreviation,Description")] LibraryCode libraryCode)
        {
            if (ModelState.IsValid) {
                _context.Add(libraryCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libraryCode);
        }

        // GET: LibraryCodes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) {
                return NotFound();
            }

            var libraryCode = await _context.LibraryCodes.FindAsync(id);
            if (libraryCode == null) {
                return NotFound();
            }
            return View(libraryCode);
        }

        // POST: LibraryCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Abbreviation,Description")] LibraryCode libraryCode)
        {
            if (id != libraryCode.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(libraryCode);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!LibraryCodeExists(libraryCode.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(libraryCode);
        }

        // GET: LibraryCodes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) {
                return NotFound();
            }

            var libraryCode = await _context.LibraryCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libraryCode == null) {
                return NotFound();
            }

            return View(libraryCode);
        }

        // POST: LibraryCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var libraryCode = await _context.LibraryCodes.FindAsync(id);
            _context.LibraryCodes.Remove(libraryCode);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibraryCodeExists(string id)
        {
            return _context.LibraryCodes.Any(e => e.Id == id);
        }
    }
}
