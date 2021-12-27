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


        public async Task<List<(LibraryCard card, (Borrow borrow, List<BookInBorrow> bibs) details)>> GetBorrows()
        {
            var ListCardAndBorrow = new List<(LibraryCard card, (Borrow borrow, List<BookInBorrow> bibs) details)>();

            var listBorrow = await _context.Borrows
                .Where(x => x.StatusBorrow == StatusBorrow.Borrowing || x.StatusBorrow == StatusBorrow.NotEnough).ToListAsync();
            //var borrows = new List<BorrowVM>();
            foreach (var borrow in listBorrow)
            {
                var card = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == borrow.IdCard);
                if (card is null) continue;
                var bibs = await _context.BookInBorrows
                    .Where(x => x.IdBorrow == borrow.Id && x.AmountReturn < x.AmountBorrowed)
                    .Include(x => x.Book).ToListAsync();
                //var listBook = bibs.Select(bib => bib.Book).ToList();

                ListCardAndBorrow.Add((card, (borrow, bibs)));
            }

            return ListCardAndBorrow;
        }

        public async Task<List<(LibraryCard card, (Borrow borrow, List<BookInBorrow> bibs) details )>> GetBorrows(Guid? idCard)
        {
            var ListCardAndBorrow = new List<(LibraryCard card, (Borrow borrow, List<BookInBorrow> bibs) details)>();

            var card = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == idCard);

            var listBorrow = await _context.Borrows
                .Where(  x => x.IdCard == idCard && (x.StatusBorrow == StatusBorrow.Borrowing || x.StatusBorrow == StatusBorrow.NotEnough || x.StatusBorrow == StatusBorrow.Missing)).ToListAsync();
            foreach (var borrow in listBorrow) {
                var bibs = await _context.BookInBorrows
                    .Where(x => x.IdBorrow == borrow.Id && x.AmountReturn < x.AmountBorrowed)
                    .Include(x => x.Book).ToListAsync();

                ListCardAndBorrow.Add((card, (borrow,bibs)));
            }

            return ListCardAndBorrow;
        }

        public async Task<List<(LibraryCard card, (Borrow borrow, List<BookInBorrow> bibs) details)>> GetBorrowsWithStudentCode(string content)
        {
            var ListCardAndBorrow = new List<(LibraryCard card, (Borrow borrow, List<BookInBorrow> bibs) details)>();

            var card = await _context.LibraryCards.FirstOrDefaultAsync(x => x.MSSV.Contains(content) || x.Name.Contains(content) );
            if (card == null)
            {
                return ListCardAndBorrow;
            }
            var listBorrow = await _context.Borrows
                .Where(x => x.IdCard == card.Id && (x.StatusBorrow == StatusBorrow.Borrowing || x.StatusBorrow == StatusBorrow.NotEnough || x.StatusBorrow == StatusBorrow.Missing)).ToListAsync();
            foreach (var borrow in listBorrow) {
                var bibs = await _context.BookInBorrows
                    .Where(x => x.IdBorrow == borrow.Id && x.AmountReturn < x.AmountBorrowed)
                    .Include(x => x.Book).ToListAsync();

                ListCardAndBorrow.Add((card, (borrow, bibs)));
            }

            return ListCardAndBorrow;
        }

        public async Task<ReturnBookVM> GetBorrow(Guid idBorrow)
        {
            var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.Id == idBorrow && (x.StatusBorrow != StatusBorrow.Finish));
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

        public async Task<ReturnBookVM> GetBorrowWithCard(Guid idCard)
        {
            var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.IdCard == idCard && x.StatusBorrow != StatusBorrow.Finish);
            //var borrows = new List<BorrowVM>();
            if (borrow is null)
                return null;
            var card = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == borrow.IdCard);
            if (card is null) return null;

            var bibs = await _context.BookInBorrows
                .Where(x => x.IdBorrow == borrow.Id && x.AmountReturn < x.AmountBorrowed)
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

        //public async Task<ReturnBookVM> GetBorrowWithCard(Guid idCard, Guid idBorrow)
        //{
        //    Borrow borrow;
        //    if (idBorrow != Guid.Empty)
        //        borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.IdCard == idCard && x.Id == idBorrow && x.StatusBorrow != StatusBorrow.Finish);
        //    else {
        //        borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.IdCard == idCard && x.StatusBorrow != StatusBorrow.Finish);
        //    }
        //    //var borrows = new List<BorrowVM>();
        //    if (borrow is null)
        //        return null;
        //    var card = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == borrow.IdCard);
        //    if (card is null) return null;

        //    var bibs = await _context.BookInBorrows
        //        .Where(x => x.IdBorrow == borrow.Id && x.AmountReturn < x.AmountBorrowed)
        //        .Include(x => x.Book).ToListAsync();

        //    var retuenBook = new ReturnBookVM() {
        //        IdCard = card.Id,
        //        IdBorrow = borrow.Id,
        //        BookInBorrows = bibs,
        //        LibraryCard = card
        //    };

        //    return retuenBook;
        //}

        public async Task<List<Borrow>> GetBorrowsWithCard(Guid idCard)
        {
            var borrow = await _context.Borrows.Where(x => x.IdCard == idCard && x.StatusBorrow != StatusBorrow.Finish).ToListAsync();
            return borrow;

        }

        public async Task<(bool result, string mess)> CheckRankBorrow(Borrow request, List<string> idBooks)
        {
            var libcrad = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == request.IdCard);
            if (libcrad is null)
                return (false, "Thẻ mượn không tìm thấy");
            foreach (var idBook in idBooks)
            {
                var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == idBook && x.Amount > 0);
                if(book.Rank > libcrad.Rank)
                    return (false, $"Thẻ không đủ đẳng cấp để mượn sách <{book.Name}>");
            }

            return (true, "");
        }

        public async Task<(bool isSuccess, BorrowVM borrow)> PostBorrow(Borrow request, List<string> idBooks)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.IdUser);
            if (user is null)
                return (false, null);
            var libcrad = await _context.LibraryCards.FirstOrDefaultAsync(x => x.Id == request.IdCard);
            if (libcrad is null)
                return (false, null);

            if (libcrad.StatusCard == StatusCard.Borrowed)
                return (false, null);

            var idBorrow = Guid.Empty;

            if (libcrad.StatusCard == StatusCard.Borrowed)
            {
                var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.IdCard == libcrad.Id);
                idBorrow = borrow.Id;
                borrow.StatusBorrow = StatusBorrow.Borrowing;
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
                var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == idBook && x.Amount > 0);
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
                }

                libcrad.Exp++;
                libcrad.Rank = UpRank(libcrad);
                await _context.SaveChangesAsync();

            }
            return (true, null);
        }

        RankLibrary UpRank(LibraryCard libraryCard)
        {

            var expUp = new List<int> {10, 21, 32, 43, 54, 65, 85};

            var currentRank = (int) libraryCard.Rank;

            var expNeedUp = expUp[currentRank];

            if (libraryCard.Exp > expNeedUp && currentRank < 5)
            {
                libraryCard.Rank = (RankLibrary) (currentRank + 1);
                libraryCard.ExpLevelUp = expUp[currentRank + 1];
            }

            return libraryCard.Rank;
        }

        public async Task<bool> ReturnBookAll(Guid idCard, Guid idBorrow)
        {
            //<truy xuất dữ liệu>
            var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.Id == idBorrow);
            if (borrow is null)
                return false;


            var bibs = await _context.BookInBorrows.Where(x => x.IdBorrow == idBorrow).ToListAsync();

            var card = await _context.LibraryCards.FindAsync(idCard);
            //</truy xuất dữ liệu>
            
            foreach (var item in bibs) {
                var amount = item.AmountBorrowed - item.AmountReturn - item.AmountMissing;

                //Cộng lại số lượng sách tồn kho
                var book = await _context.Books.FindAsync((item.IdBook));
                if (book != null) {
                    book.Amount += amount;
                }

                item.AmountReturn = amount;
                item.TimeRealReturn = DateTime.Now;
            }

            borrow.AmountReturned = borrow.AmountBorrow;
            borrow.StatusBorrow = StatusBorrow.Finish;

            card.StatusCard = StatusCard.CanBorrow;

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> ReturnBook(ReturnBookRequest request)
        {
            var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.Id == request.IdBorrow);
            if (borrow is null)
                return false;


            var bib = await _context.BookInBorrows.FirstOrDefaultAsync(x => x.IdBook == request.IdBook && x.IdBorrow == request.IdBorrow);
            if(bib is null) return false;

            var card = await _context.LibraryCards.FindAsync(request.IdCard);

            var amount = bib.AmountBorrowed - bib.AmountReturn - bib.AmountMissing;

            if (request.AmountReturn > amount)
            {
                request.AmountReturn = amount;
            }

            //Cộng lại số lượng sách tồn kho
            var book = await _context.Books.FindAsync((bib.IdBook));
            if (book != null)
            {
                book.Amount += request.AmountReturn;
            }


            bib.AmountReturn += request.AmountReturn;

            // Xử lý lỗi: nếu số lượng sách đang mượn bé hơn số sách trả thì coi như trả hết.
            if (bib.AmountBorrowed < bib.AmountReturn) {
                bib.AmountReturn = bib.AmountBorrowed;
            }

            // Xử lý lỗi: nếu số sách trả cộng với số sách bị mất mà lớn hơn số sách đã mượn thì phải trừ lại số sách trả.
            //if (bib.AmountBorrowed < bib.AmountReturn + bib.AmountMissing)
            //{
            //    // Số sách trả trừ cho số sách đã mất
            //    bib.AmountReturn -= bib.AmountMissing;
            //}

            // Cập nhật tổng số sách mất
            //borrow.AmountMissing += bib.AmountMissing;

            // Cập nhật vào tổng số sách trả được
            borrow.AmountReturned += request.AmountReturn;


            bib.TimeRealReturn = DateTime.Now;

            // Xử lý lỗi: nếu tổng số lượng sách đang mượn bé hơn số sách trả thì coi như trả hết.
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


        public async Task<bool> MissingBook(ReturnBookRequest request)
        {
            var card = await _context.LibraryCards.FindAsync(request.IdCard);

            var bib = await _context.BookInBorrows.FirstOrDefaultAsync(x =>
                x.IdBorrow == request.IdBorrow && x.IdBook == request.IdBook);

            var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.Id == request.IdBorrow);

            if (bib == null)
                return false;

            var amount = bib.AmountBorrowed - bib.AmountReturn - bib.AmountMissing;

            if (request.AmountReturn > amount) {
                request.AmountReturn = amount;
            }

            //Sử dụng chung AmountReturn trên giao diện
            bib.AmountMissing += request.AmountReturn;

            bib.TimeMissing=DateTime.Now;

            if (bib.AmountBorrowed <= bib.AmountMissing)
                bib.AmountMissing = bib.AmountBorrowed;

            borrow.AmountMissing += request.AmountReturn;
            var amountBorrow = borrow.AmountBorrow - borrow.AmountReturned - borrow.AmountMissing ;
            if (amountBorrow <= 0)
            {
                card.StatusCard = StatusCard.CanBorrow;
            }
            borrow.StatusBorrow = StatusBorrow.Missing;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnMissingBook(ReturnBookRequest request)
        {
            var card = await _context.LibraryCards.FindAsync(request.IdCard);


            var bib = await _context.BookInBorrows.FirstOrDefaultAsync(x =>
                x.IdBorrow == request.IdBorrow && x.IdBook == request.IdBook);

            var borrow = await _context.Borrows.FirstOrDefaultAsync(x => x.Id == request.IdBorrow);

            if (bib == null)
                return false;
            //var amount = bib.AmountBorrowed - bib.AmountReturn - bib.AmountMissing;
            if (request.AmountReturn > bib.AmountMissing) {
                request.AmountReturn = bib.AmountMissing;
            }

            //Cộng lại số lượng sách tồn kho
            var book = await _context.Books.FindAsync((bib.IdBook));
            if (book != null) {
                book.Amount += request.AmountReturn;
            }

            //Sử dụng chung AmountReturn trên giao diện
            bib.AmountMissing -= request.AmountReturn;

            if (bib.AmountMissing <= 0)
            {
                bib.AmountMissing = 0;
                bib.TimeMissing = null;
            }

            bib.AmountReturn += request.AmountReturn;

            bib.TimeMissing = DateTime.Now;
            

            if (bib.AmountBorrowed <= bib.AmountMissing)
                bib.AmountMissing = bib.AmountBorrowed;

            borrow.AmountReturned += request.AmountReturn;
            borrow.AmountMissing -= request.AmountReturn;

            if (borrow.AmountReturned >= borrow.AmountBorrow)
            {
                card.StatusCard = StatusCard.CanBorrow;
                borrow.StatusBorrow = StatusBorrow.Finish;
            } else
                borrow.StatusBorrow = StatusBorrow.Missing;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
