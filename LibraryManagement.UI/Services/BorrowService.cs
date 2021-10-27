using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.Requests;
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


        public async Task<List<BorrowVM>> GetBorrows()
        {
            var listBorrow = await _context.Borrows.ToListAsync();
            var borrows = new List<BorrowVM>();
            foreach (var borrow in listBorrow)
            {
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
            }

            return borrows;
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

            request.Id = Guid.NewGuid();
            request.UserName = user.Nickname;
            _context.Borrows.Add(request);
            await _context.SaveChangesAsync();

            var bibs = new List<BookInBorrow>();
            foreach (var idBook in idBooks)
            {
                var book = await _context.Books.FindAsync(idBook);
                if(book is null) continue;
                var bib = await _context.BookInBorrows.FirstOrDefaultAsync(x => x.IdBook == idBook);
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
                        IdBorrow = request.Id,
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
