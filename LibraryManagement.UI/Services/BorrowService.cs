using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.Requests;
using Library.Library.Enums;
using LibraryManagement.UI.Models.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.UI.Services
{
    public class BorrowService
    {
        private readonly LibraryDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        private readonly IConfiguration _config; //lấy config từ appsetting.config

        public BorrowService(LibraryDbContext context,

            IStorageService storageService,
            IConfiguration config)
        {
            _context = context;
            _storageService = storageService;
            _config = config;
        }


        public async Task<List<(LibraryCard card, List<BookInBorrow> bibs)>> GetBorrows()
        {
            var listBorrow = await _context.Borrows
                .Where(x => x.StatusBorrow == StatusBorrow.Borrowed).ToListAsync();
            //var borrows = new List<BorrowVM>();
            var ListCardAndBorrow = new List<(LibraryCard card, List<BookInBorrow> bibs)> ();
            foreach (var borrow in listBorrow)
            {
                var card = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == borrow.IdCard);
                if(card is null) continue;
                var bibs = await _context.BookInBorrows
                    .Where(x => x.IdBorrow == borrow.Id)
                    .Include(x => x.Book).ToListAsync();
                //var listBook = bibs.Select(bib => bib.Book).ToList();

                ListCardAndBorrow.Add((card, bibs));
            }

            return ListCardAndBorrow;
        }

        public async Task<BorrowVM> GetBorrow(Guid idBorrow)
        {
            var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.Id == idBorrow);

            var borrowView = borrow.ToViewModel();

            if (borrow.IdCard != Guid.Empty)
            {
                var libraryCard = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == borrow.IdCard);
                borrowView.LibraryCard = libraryCard;
            }

            var books = await (from bib in _context.BookInBorrows
                               join book in _context.Books on bib.IdBook equals book.Id
                               select book).ToListAsync();
            borrowView.Books = books;

            return borrowView;
        }


        public async Task<(bool isSuccess, BorrowVM borrow)> PostBorrow(Borrow request, List<string> idBooks)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.IdUser);
            if (user is null)
                return(false, null);
            var libcrad = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == request.IdCard);
            if (libcrad is null)
                return (false, null);

            var idBorrow = Guid.Empty;

            if (libcrad.StatusCard == StatusCard.Borrowed)
            {
                var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.IdCard == libcrad.Id);
                idBorrow = borrow.Id;
            }
            else
            {
                request.Id = Guid.NewGuid();
                idBorrow = request.Id;

                request.UserName = user.Nickname;
                request.AmountBorrow += idBooks.Count;
                _context.Borrows.Add(request);

                libcrad.StatusCard = StatusCard.Borrowed;

                await _context.SaveChangesAsync();
            }


            var bibs = new List<BookInBorrow>();
            foreach (var idBook in idBooks)
            {
                var book = await _context.Books.FindAsync(idBook);
                if(book is null) continue;
                var bib = await _context.BookInBorrows.FirstOrDefaultAsync(x => x.IdBook == idBook && x.IdBorrow == idBorrow);
                book.StatusBook = StatusBook.Borrowed;
                if (bib is not null)
                {
                    bib.AmountBorrowed += 1;
                }
                else
                {
                    bibs.Add(new BookInBorrow()
                    {
                        AmountBorrowed = 1,
                        IdBook = idBook,
                        IdBorrow = idBorrow,
                        TimeBorrowed = DateTime.Now,
                        TimeReturn = DateTime.Now.AddDays(book.DateCanBorrow) 
                    });
                }
            }
            await _context.BookInBorrows.AddRangeAsync(bibs);
            await _context.SaveChangesAsync();
            return (true, null);
        }
    }
}
