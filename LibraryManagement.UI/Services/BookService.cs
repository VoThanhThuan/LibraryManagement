using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.Requests;
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

        public async Task<List<Book>> GetBooks()
        {
            var books = _context.Books.Include(b => b.LibraryCode);

            var host = _config.GetSection("BaseAddress").Value;
            foreach (var book in books)
                book.Thumbnail = $"{host}/{book.Thumbnail}";
            return await books.ToListAsync();
        }

        public async Task<Book> GetBook(string id)
        {
            var book = await _context.Books
                .Include(b => b.LibraryCode)
                .FirstOrDefaultAsync(m => m.Id == id);
            var host = _config.GetSection("BaseAddress").Value;
            book.Thumbnail = $"{host}/{book.Thumbnail}";
            return book;
        }

        public async Task<Book> PutBook(BookRequest request)
        {
            var book = request.ToBook();

            if (request.Thumbnail is not null)
            {
                book.Thumbnail = await _storageService.SaveFileAsync(request.Thumbnail, "books");
            }

            _context.Books.Update(book);
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
