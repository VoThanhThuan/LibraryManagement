﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Library.Data;
using Library.Library.Entities.ViewModels;
using Library.Library.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UI.Services
{
    public class StatisticalService
    {
        private readonly LibraryDbContext _context;

        public StatisticalService(LibraryDbContext context)
        {
            _context = context;
        }

        private async Task<StatisticalVM> GetAll()
        {
            var booksBorrowed = await (from bib in _context.BookInBorrows
                                       join b in _context.Books on bib.IdBook equals b.Id
                                       join br in _context.Borrows on bib.IdBorrow equals br.Id
                                       join lc in _context.LibraryCards on br.IdCard equals lc.Id
                                       select b.ToViewModel(bib.ToViewModel(), bib.AmountBorrowed, lc.ToViewModel())).ToListAsync();

            var booksBorrowing = await (from br in _context.Borrows
                                        where br.StatusBorrow == StatusBorrow.Borrowing || br.StatusBorrow == StatusBorrow.NotEnough
                                        join bib in _context.BookInBorrows on br.Id equals bib.IdBorrow
                                        join b in _context.Books on bib.IdBook equals b.Id
                                        join lc in _context.LibraryCards on br.IdCard equals lc.Id
                                        select b.ToViewModel(bib.ToViewModel(), bib.AmountBorrowed, lc.ToViewModel())).ToListAsync();

            var booksReturn = await (from br in _context.Borrows where br.StatusBorrow == StatusBorrow.Finish || br.StatusBorrow == StatusBorrow.NotEnough
                                     join bib in _context.BookInBorrows on br.Id equals bib.IdBorrow
                                     join b in _context.Books on bib.IdBook equals b.Id
                                     join lc in _context.LibraryCards on br.IdCard equals lc.Id
                                     select b.ToViewModel(bib.ToViewModel(), bib.AmountReturn, lc.ToViewModel())).ToListAsync();

            //var booksMissing = await _context.Books.Where(x => x.StatusBook == StatusBook.Missing).Select(x => x.ToViewModel()).ToListAsync();

            var booksMissing = await (from bib in _context.BookInBorrows
                                      where bib.AmountMissing > 0
                                      join b in _context.Books on bib.IdBook equals b.Id
                                      join br in _context.Borrows on bib.IdBorrow equals br.Id
                                      join lc in _context.LibraryCards on br.IdCard equals lc.Id
                                      select b.ToViewModel(bib.ToViewModel(), bib.AmountMissing, lc.ToViewModel())).ToListAsync();

            var booksTop = await _context.Books.OrderByDescending(x => x.TotalBorrow)
                .Select(x => x.ToViewModel()).ToListAsync();

            var booksReturnNotEnough = await (from br in _context.Borrows
                                              where br.StatusBorrow == StatusBorrow.NotEnough
                                              join bib in _context.BookInBorrows on br.Id equals bib.IdBorrow
                                              join b in _context.Books on bib.IdBook equals b.Id
                                              join lc in _context.LibraryCards on br.IdCard equals lc.Id
                                              select b.ToViewModel(bib.ToViewModel(), bib.AmountBorrowed - bib.AmountReturn, lc.ToViewModel())).ToListAsync();

            var cardsTop = await _context.LibraryCards.OrderByDescending(x => x.Exp).ToListAsync();

            var cardReturnLate = await (from bib in _context.BookInBorrows
                                        where bib.TimeReturn < DateTime.Now
                                        join borrow in _context.Borrows on bib.IdBorrow equals borrow.Id
                                        join card in _context.LibraryCards on borrow.IdCard equals card.Id
                                        select card).Distinct().ToListAsync();
            ;

            var cardLock = await _context.LibraryCards.Where(x => x.IsLock).ToListAsync();

            var statistical = new StatisticalVM() {
                BooksBorrowed = booksBorrowed,
                BooksBorrowing = booksBorrowing,
                BooksMissing = booksMissing,
                BooksReturn = booksReturn,
                CardsLock = cardLock,
                CardReturnLate = cardReturnLate,
                ReturnNotEnoughBooks = booksReturnNotEnough,
                TopBooks = booksTop,
                TopLibraryCards = cardsTop

            };
            return statistical;
        }

        private async Task<StatisticalVM> GetWithDate(DateTime start, DateTime end)
        {
            var booksBorrowed = await (from bib in _context.BookInBorrows where bib.TimeBorrowed >= start && bib.TimeBorrowed <= end
                                       join b in _context.Books on bib.IdBook equals b.Id
                                       join br in _context.Borrows on bib.IdBorrow equals br.Id
                                       join lc in _context.LibraryCards on br.IdCard equals lc.Id
                                       select b.ToViewModel(bib.ToViewModel(), bib.AmountBorrowed, lc.ToViewModel())).ToListAsync();

            var booksBorrowing = await (from br in _context.Borrows
                                        where br.StatusBorrow == StatusBorrow.Borrowing || br.StatusBorrow == StatusBorrow.NotEnough
                                        join bib in _context.BookInBorrows on br.Id equals bib.IdBorrow where bib.TimeBorrowed >= start && bib.TimeBorrowed <= end
                                        join b in _context.Books on bib.IdBook equals b.Id
                                        join lc in _context.LibraryCards on br.IdCard equals lc.Id
                                        select b.ToViewModel(bib.ToViewModel(), bib.AmountBorrowed, lc.ToViewModel())).ToListAsync();

            var booksReturn = await (from br in _context.Borrows where br.StatusBorrow == StatusBorrow.Finish
                                     join bib in _context.BookInBorrows on br.Id equals bib.IdBorrow where bib.TimeBorrowed >= start && bib.TimeBorrowed <= end
                                     join b in _context.Books on bib.IdBook equals b.Id
                                     join lc in _context.LibraryCards on br.IdCard equals lc.Id
                                     select b.ToViewModel(bib.ToViewModel(), bib.AmountReturn, lc.ToViewModel())).ToListAsync();

            var booksMissing = await (from bib in _context.BookInBorrows
                                      where bib.AmountMissing > 0 && (bib.TimeMissing >= start && bib.TimeMissing <= end)
                                      join b in _context.Books on bib.IdBook equals b.Id
                                      join br in _context.Borrows on bib.IdBorrow equals br.Id
                                      join lc in _context.LibraryCards on br.IdCard equals lc.Id
                                      select b.ToViewModel(bib.ToViewModel(), bib.AmountMissing, lc.ToViewModel())).ToListAsync();
            
            var a = await _context.Borrows.Where(x => x.StatusBorrow == StatusBorrow.NotEnough).ToListAsync();

            var booksReturnNotEnough = await (from br in _context.Borrows
                                              where br.StatusBorrow == StatusBorrow.NotEnough
                                              join bib in _context.BookInBorrows on br.Id equals bib.IdBorrow
                                              where bib.TimeBorrowed >= start & bib.TimeBorrowed <= end
                                              join b in _context.Books on bib.IdBook equals b.Id
                                              join lc in _context.LibraryCards on br.IdCard equals lc.Id
                                              select b.ToViewModel(bib.ToViewModel(), bib.AmountBorrowed - bib.AmountReturn, lc.ToViewModel())).ToListAsync();

            var booksTop = await _context.Books.OrderByDescending(x => x.TotalBorrow)
                .Select(x => x.ToViewModel()).ToListAsync();


            var cardsTop = await _context.LibraryCards.OrderByDescending(x => x.Exp).ToListAsync();

            var cardReturnLate = await (from bib in _context.BookInBorrows
                                        where bib.TimeReturn < DateTime.Now && (bib.TimeReturn >= start & bib.TimeReturn <= end)
                                        join borrow in _context.Borrows on bib.IdBorrow equals borrow.Id
                                        join card in _context.LibraryCards on borrow.IdCard equals card.Id
                                        select card).Distinct().ToListAsync();
            ;

            var cardLock = await _context.LibraryCards.Where(x => x.IsLock).ToListAsync();

            var statistical = new StatisticalVM() {
                BooksBorrowed = booksBorrowed,
                BooksBorrowing = booksBorrowing,
                BooksMissing = booksMissing,
                BooksReturn = booksReturn,
                CardsLock = cardLock,
                CardReturnLate = cardReturnLate,
                ReturnNotEnoughBooks = booksReturnNotEnough,
                TopBooks = booksTop,
                TopLibraryCards = cardsTop

            };
            return statistical;
        }


        public async Task<StatisticalVM> GetStatiscals(DateTime? start = null, DateTime? end = null)
        {
            var statistical = new StatisticalVM();

            if (start != null && end != null) {
                statistical = await GetWithDate((DateTime)start, (DateTime)end);
            } else {
                statistical = await GetAll();
            }
            return statistical;
        }

    }
}
