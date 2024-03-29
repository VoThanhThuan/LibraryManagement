﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.Requests;
using Library.Library.Entities.ViewModels;
using Library.Library.Enums;
using LibraryManagement.UI.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.UI.Services
{
    public class BookService
    {
        private readonly LibraryDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config; //lấy config từ appsetting.config

        public BookService(LibraryDbContext context, IStorageService storageService, IConfiguration config)
        {
            _context = context;
            _storageService = storageService;
            _config = config;
        }

        public async Task<List<BookVM>> GetBooks()
        {
            var books = await _context.Books.Include(x => x.LibraryCode).Select(x => x.ToViewModel()).ToListAsync();
            return books;
        }

        public async Task<BookVM> GetBook(string id)
        {
            var book = (await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id)).ToViewModel();
            return book ?? null;
        }

        public async Task<List<BookVM>> SearchBook(string content)
        {
            if (content == "!all")
            {
                return await _context.Books.Where(x => x.Amount > 0).Select(x => x.ToViewModel()).ToListAsync();
            }

            var books = await _context.Books
                .Where(x => x.Amount > 0 && (x.Id.ToLower().Contains(content.ToLower()) || x.Name.ToLower().Contains(content.ToLower())))
                .Take(10)
                .Select(x => x.ToViewModel()).ToListAsync();

            return books;
        }

        public async Task<List<BookVM>> GetBorrowedBooks()
        {
            var books = await (from bib in _context.BookInBorrows
                join b in _context.Books on bib.IdBook equals b.Id
                select b.ToViewModel()).ToListAsync();
            return books;
        }

        public async Task<List<BookVM>> GetBorrowingBooks()
        {
            var books = await (from br in _context.Borrows
                where br.StatusBorrow == StatusBorrow.Borrowing
                join bib in _context.BookInBorrows on br.Id equals bib.IdBorrow
                join b in _context.Books on bib.IdBook equals b.Id
                select b.ToViewModel()).ToListAsync();
            return books;
        }

        public async Task<List<BookVM>> GetReturnBooks()
        {
            var books = await (from br in _context.Borrows where br.StatusBorrow == StatusBorrow.Finish 
                join bib in _context.BookInBorrows on br.Id equals bib.IdBorrow
                join b in _context.Books on bib.IdBook equals b.Id
                select b.ToViewModel()).ToListAsync();
            return books;
        }

        public async Task<List<BookVM>> GetReturnNotEnoughBooks()
        {
            var books = await (from br in _context.Borrows
                where br.StatusBorrow == StatusBorrow.NotEnough
                join bib in _context.BookInBorrows on br.Id equals bib.IdBorrow
                join b in _context.Books on bib.IdBook equals b.Id
                select b.ToViewModel()).ToListAsync();
            return books;
        }

        public async Task<List<BookVM>> GetTopBooks()
        {
            var books = await _context.Books.OrderByDescending(x => x.TotalBorrow).Select(x => x.ToViewModel()).ToListAsync();

            return books;
        }


        public async Task<Book> PutBook(BookRequest request)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (book is null) return null;

            if (request.Thumbnail is not null)
            {
                await _storageService.DeleteFileAsync(book.Thumbnail);
                book.Thumbnail = await _storageService.SaveFileAsync(request.Thumbnail, "books");
            }

            book.Name = request.Name;
            book.PublishingCompany = request.PublishingCompany;
            book.PublicationDate = request.PublicationDate;
            book.Author = request.Author;
            book.Amount = request.Amount;
            book.PageNumber = request.PageNumber;
            book.DateCanBorrow = request.DateCanBorrow;
            book.Rank = request.Rank;
            book.IdLibraryCode = request.IdLibraryCode;
            book.StatusBook = request.StatusBook;

                //_context.Books.Update(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<Book> PostBook(BookRequest request)
        {
            var book = request.ToBook();

            if(request.Thumbnail is not null)
                book.Thumbnail = await _storageService.SaveFileAsync(request.Thumbnail, "books");

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<int> DeleteBook(string id)
        {
            var book = await _context.Books.FindAsync(id);

            var result = await _storageService.DeleteFileAsync(book.Thumbnail);
            if (result is not 200)
                return 500;
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return 200;
            
        }


    }
}
