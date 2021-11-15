using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Library.Library.Data;
using Library.Library.Entities.ViewModels;
using LibraryManagement.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.UI.Controllers {
    [Authorize]
    public class HomeController : Controller {

        private readonly LibraryDbContext _context;
        private readonly BookService _book;
        private readonly GenreService _genre;
        private readonly LibraryCardService _card;

        public HomeController(LibraryDbContext context, BookService book, GenreService genre, LibraryCardService card) {
            _context = context;
            _book = book;
            _genre = genre;
            _card = card;
        }

        public async Task<IActionResult> Index() {
            var statistical = new StatisticalVM {
                TotalBookBorrowed = (await _book.GetBorrowedBooks()).Count,
                TotalBookBorrowing = (await _book.GetBorrowingBooks()).Count,
                TotalBookReturn = (await _book.GetReturnBooks()).Count,
                TopBooks = await _book.GetTopBooks(),
                ReturnNotEnoughBooks = await _book.GetReturnNotEnoughBooks()       ,
                TopLibraryCards = await _card.TopLibraryCards()
            };
            //statistical.CradReturnLate;


            return View(statistical);
        }
    }
}
