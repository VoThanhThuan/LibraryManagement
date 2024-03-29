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
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.UI.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index(Guid? idCard = null)
        {
            //var libraryDbContext = _context.Borrows.Include(b => b.LibraryCard);
            var ListCardAndBorrow = new List<(LibraryCard card, (Borrow borrow, List<BookInBorrow> bibs) details)>();
            if (idCard != null) {
                ListCardAndBorrow = await _borrow.GetBorrows(idCard);
                ViewData["borrow-on"] = "On";
            } else {
                ListCardAndBorrow = await _borrow.GetBorrows();
            }
            return View(ListCardAndBorrow);
        }

        [HttpGet("Search")]
        // GET: Borrows
        public async Task<IActionResult> Search(string content)
        {
            //var libraryDbContext = _context.Borrows.Include(b => b.LibraryCard);
            var ListCardAndBorrow = new List<(LibraryCard card, (Borrow borrow, List<BookInBorrow> bibs) details)>();
            ListCardAndBorrow = await _borrow.GetBorrowsWithStudentCode(content);
            ViewData["borrow-on"] = "On";

            return View("Index", ListCardAndBorrow);
        }

        [HttpGet("Details")]
        // GET: Borrows/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) {
                return RedirectToAction(nameof(Index));
            }

            var borrow = await _context.Borrows
                .Include(b => b.LibraryCard)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null) {
                return RedirectToAction(nameof(Index));

            }

            return View(borrow);
        }

        // GET: Borrows/Create
        // GET: Borrows/Create/xxx-xxx-xxx
        [HttpGet("Create/{idCard}")]
        public async Task<IActionResult> Create(Guid idCard)
        {
            ViewData["Id-User"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Name-User"] = (User.FindFirstValue(ClaimTypes.Name));

            var libCard = await _context.LibraryCards.FindAsync(idCard);
            if (libCard.IsLock) {
                TempData["success"] = $"Thẻ của {libCard.Name} đã bị khóa";
                return Redirect("/");
            }

            ViewBag.LibraryCard = libCard;


            var borrow = new Borrow();

            //if (idCard is not null)
            //{

            //    if (cardSelect is not null)
            //    {
            //        ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "MSSV", cardSelect.Id);
            //        borrow.IdCard = cardSelect.Id;
            //    }

            //}
            //else
            //{
            //    ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "MSSV");
            //}


            var books = await _book.GetBooks();

            return View(borrow);
        }

        // POST: Borrows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create/{idCard}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateBorrow,Note,IdUser,UserName,IdCard")] Borrow borrow, List<string> idBooks, Guid idCard)
        {
            if (!idBooks.Any())
                return RedirectToAction(nameof(Index));

            ViewData["Id-User"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Name-User"] = (User.FindFirstValue(ClaimTypes.Name));
            var libCard = await _context.LibraryCards.FindAsync(idCard);
            ViewBag.LibraryCard = libCard;

            var check = await _borrow.CheckRankBorrow(borrow, idBooks);
            if (!check.result) {
                TempData["error"] = check.mess;
                return View(borrow);
            }

            var isSuccess = false;

            if (ModelState.IsValid) {
                var result = await _borrow.PostBorrow(borrow, idBooks);
                if (!result.isSuccess) {
                    TempData["error"] = $"Lỗi mượn";
                    return Redirect("/Borrows");
                }
                isSuccess = result.isSuccess;
            }

            if (libCard.IsLock) {
                TempData["success"] = $"Thẻ của {libCard.Name} đã bị khóa";
                return Redirect("/LibraryCards");
            }

            //ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "Id", borrow.IdCard);
            var books = await _book.GetBooks();
            if (isSuccess) {
                TempData["success"] = $"Đã thêm lịch mượn cho đọc giả {libCard.Name} thành công";
                return RedirectToAction(nameof(Index));
            } else {
                return View(borrow);
            }
        }

        [HttpGet("ReturnBook")]
        public async Task<IActionResult> ReturnBook(Guid idCard, Guid idBorrow)
        {
            var cardAndBook = new ReturnBookVM();

            var card = _context.LibraryCards.Find(idCard);

            if (card is null)
                return RedirectToAction(nameof(Index));

            if (idBorrow == Guid.Empty) {
                var borrows = await _borrow.GetBorrowsWithCard(idCard);
                if (borrows.Count > 1) {
                    return Redirect($"/Borrows?idCard={idCard}");
                }
            }


            ViewBag.LibraryCard = card;
            cardAndBook.IdCard = idCard;
            if (idBorrow != Guid.Empty) {
                cardAndBook = await _borrow.GetBorrow(idBorrow);
                return View(cardAndBook);
            } else if (idCard != Guid.Empty) {
                cardAndBook = await _borrow.GetBorrowWithCard(idCard);
                if (cardAndBook is null)
                    return RedirectToAction(nameof(Index));

                return View(cardAndBook);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("ReturnBookAll")]
        public async Task<IActionResult> ReturnBookAll(Guid idCard, Guid idBorrow)
        {

            if (idCard != Guid.Empty) {
                var resultGet = await _borrow.GetBorrowWithCard(idCard);

                resultGet.IdCard = idCard;
                //cardAndBook = resultGet;
            }
            //else if (request.IdBorrow != Guid.Empty)
            //{
            //    cardAndBook = await _borrow.GetBorrow(request.IdBorrow);
            //}

            var result = await _borrow.ReturnBookAll(idCard, idBorrow);
            if (result == false) {
                TempData["error"] = $"Đã trả toàn bộ sách thất bại";
                return RedirectToAction(nameof(ReturnBook));

            }
            TempData["success"] = $"Đã trả toàn bộ sách thành công";
            return Redirect("/Borrows");

        }

        [HttpPost("api/ReturnBook")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(ReturnBookRequest request)
        {
            if (request.AmountReturn <= 0) return BadRequest("Số lượng trả không được bé hơn 1");
            //var cardAndBook = new ReturnBookVM();
            //ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "Id", request.IdCard);

            if (request.IdCard != Guid.Empty) {
                var resultGet = await _borrow.GetBorrowWithCard(request.IdCard);

                resultGet.IdCard = request.IdCard;
                //cardAndBook = resultGet;
            }

            var result = await _borrow.ReturnBook(request);
            if (result == false) {
                //return View(cardAndBook);
                return View($"/Borrows/ReturnBook?idCard={request.IdCard}&&idBorrow={request.IdBook}");

            }
            TempData["success"] = $"Đã trả {request.AmountReturn} cuốn sách thành công";
            return Redirect("/Borrows");
        }

        [HttpPost("api/MissingBook")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MissingBook(ReturnBookRequest request)
        {
            if (request.AmountReturn <= 0) return BadRequest("Số lượng báo mất không được bé hơn 1");
            var result = await _borrow.MissingBook(request);
            if (!result)
                return Conflict("Số lượng báo mất không được bé hơn 1");
            return Ok("Báo mất thành công");
        }

        [HttpPost("api/ReturnMissingBook")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnMissingBook(ReturnBookRequest request)
        {
            if (request.AmountReturn <= 0) return BadRequest("Số lượng trả không được bé hơn 1");
            var result = await _borrow.ReturnMissingBook(request);
            if (!result)
                return Conflict("Số lượng trả không được bé hơn 1");
            TempData["success"] = $"Đã trả {request.AmountReturn} bị mất thành công";
            return Ok("Trả sách báo mất thành công thành công");
        }

        [HttpGet("Edit")]
        // GET: Borrows/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) {
                TempData["error"] = " không tìm thấy <id>";
                return RedirectToAction(nameof(Index));
            }

            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow == null) {
                TempData["error"] = " không tìm thấy <id>";
                return RedirectToAction(nameof(Index));
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
            if (id != borrow.Id) {
                TempData["error"] = " không tìm thấy <id>";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(borrow);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!BorrowExists(borrow.Id)) {
                        TempData["error"] = " không tìm thấy <id>";
                        return RedirectToAction(nameof(Index));
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["success"] = $"Cập nhật thành công";
            ViewData["IdCard"] = new SelectList(_context.LibraryCards, "Id", "Id", borrow.IdCard);
            return View(borrow);
        }

        [HttpGet("Delete")]
        // GET: Borrows/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) {
                TempData["error"] = "id bị null";
                return RedirectToAction(nameof(Index));
            }

            var borrow = await _context.Borrows
                .Include(b => b.LibraryCard)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null) {
                TempData["error"] = "Xóa thất bại";
                return RedirectToAction(nameof(Index));
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
