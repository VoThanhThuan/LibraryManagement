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

namespace LibraryManagement.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly LibraryDbContext _context;
        private readonly BookService _book;
        private readonly GenreService _genre;

        public HomeController(LibraryDbContext context, BookService book, GenreService genre)
        {
            _context = context;
            _book = book;
            _genre = genre;
        }

        public async Task<IActionResult> Index()
        {
            var statistical = new StatisticalVM();
            statistical.TotalBookBorrowed = (await _book.GetBorrowedBooks()).Count;
            statistical.TotalBookBorrowing = (await _book.GetBorrowingBooks()).Count;
            statistical.TotalBookReturn = (await _book.GetReturnBooks()).Count;
            statistical.TopBooks = await _book.GetTopBooks();
            statistical.ReturnNotEnoughBooks = await _book.GetReturnNotEnoughBooks();
            //statistical.TopLibraryCards;
            //statistical.CradReturnLate;


            return View(statistical);
        }
    }
}
