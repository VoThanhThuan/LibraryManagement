using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.Requests;
using Library.Library.Entities.ViewModels;
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
            var ListCardAndBorrow = new List<(LibraryCard card, List<BookInBorrow> bibs)>();
            foreach (var borrow in listBorrow)
            {
                var card = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == borrow.IdCard);
                if (card is null) continue;
                var bibs = await _context.BookInBorrows
                    .Where(x => x.IdBorrow == borrow.Id)
                    .Include(x => x.Book).ToListAsync();
                //var listBook = bibs.Select(bib => bib.Book).ToList();

                ListCardAndBorrow.Add((card, bibs));
            }

            return ListCardAndBorrow;
        }

        public async Task<ReturnBookVM> GetBorrow(Guid idBorrow)
        {
            var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.Id == idBorrow && x.StatusBorrow == StatusBorrow.Borrowed);
            //var borrows = new List<BorrowVM>();

            var card = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == borrow.IdCard);
            if (card is null) return null;

            var bibs = await _context.BookInBorrows
                .Where(x => x.IdBorrow == borrow.Id)
                .Include(x => x.Book).ToListAsync();

            var retuenBook = new ReturnBookVM()
            {
                IdCard = card.Id,
                IdBorrow = borrow.Id,
                BookInBorrows = bibs,
                LibraryCard = card
            };

            return retuenBook;
        }

        public async Task<ReturnBookVM> GetBorrowWithCard(Guid idCard)
        {
            var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.IdCard == idCard && x.StatusBorrow != StatusBorrow.Finish);
            //var borrows = new List<BorrowVM>();
            if (borrow is null)
                return null;
            var card = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == borrow.IdCard);
            if (card is null) return null;

            var bibs = await _context.BookInBorrows
                .Where(x => x.IdBorrow == borrow.Id)
                .Include(x => x.Book).ToListAsync();

            var retuenBook = new ReturnBookVM()
            {
                IdCard = card.Id,
                IdBorrow = borrow.Id,
                BookInBorrows = bibs,
                LibraryCard = card
            };

            return retuenBook;
        }

        public async Task<(bool isSuccess, BorrowVM borrow)> PostBorrow(Borrow request, List<string> idBooks)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.IdUser);
            if (user is null)
                return (false, null);
            var libcrad = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == request.IdCard);
            if (libcrad is null)
                return (false, null);

            var idBorrow = Guid.Empty;

            if (libcrad.StatusCard == StatusCard.Borrowed)
            {
                var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.IdCard == libcrad.Id);
                idBorrow = borrow.Id;
                borrow.StatusBorrow = StatusBorrow.Borrowed;
            }
            else
            {
                request.Id = Guid.NewGuid();
                idBorrow = request.Id;

                request.UserName = user.Nickname;
                request.AmountBorrow += idBooks.Count;
                _context.Borrows.Add(request);

                libcrad.StatusCard = StatusCard.Borrowed;
            }
            await _context.SaveChangesAsync();


            foreach (var idBook in idBooks)
            {
                var book = await _context.Books.FindAsync(idBook);
                if (book is null) continue;
                book.TotalBorrow += 1;
                var bib = await _context.BookInBorrows.FirstOrDefaultAsync(x => x.IdBook == idBook && x.IdBorrow == idBorrow);
                book.Amount--;
                if (book.Amount < 0)
                    book.Amount = 0;
                if (book.Amount == 0)
                    book.StatusBook = StatusBook.Borrowed;
                if (bib is not null)
                {
                    bib.AmountBorrowed += 1;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var newbib = new BookInBorrow()
                    {
                        AmountBorrowed = 1,
                        IdBook = idBook,
                        IdBorrow = idBorrow,
                        TimeBorrowed = DateTime.Now,
                        TimeReturn = DateTime.Now.AddDays(book.DateCanBorrow)
                    };
                    await _context.BookInBorrows.AddAsync(newbib);
                    await _context.SaveChangesAsync();
                }
            }
            return (true, null);
        }

        public async Task<bool> ReturnBook(ReturnBookRequest request)
        {
            var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.Id == request.IdBorrow);
            if (borrow is null)
                return false;


            var bib = await _context.BookInBorrows.FirstOrDefaultAsync(x => x.IdBook == request.IdBook && x.IdBorrow == request.IdBorrow);
            if(bib is null) return false;

            var card = await _context.LibraryCards.FindAsync(request.IdCard);

            if (bib.AmountBorrowed < request.AmountReturn)
            {
                request.AmountReturn = bib.AmountBorrowed;
            }

            bib.AmountBorrowed -= request.AmountReturn;
            borrow.AmountReturned += request.AmountReturn;

            bib.TimeRealReturn = DateTime.Now;


            if (borrow.AmountReturned > borrow.AmountBorrow)
            {
                borrow.AmountReturned = borrow.AmountBorrow;
            }

            if (borrow.AmountReturned == borrow.AmountBorrow)
            {
                borrow.StatusBorrow = StatusBorrow.Finish;
                card.StatusCard = StatusCard.CanBorrow;
            }

            else if (borrow.AmountReturned < borrow.AmountBorrow)
            {
                borrow.StatusBorrow = StatusBorrow.NotEnough;
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
