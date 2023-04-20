using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Library.Data;
using Library.Library.Entities;
using LibraryManagement.UI.Models.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.UI.Services
{
    public class GenreService
    {
        private readonly LibraryDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config; //lấy config từ appsetting.config

        public GenreService(LibraryDbContext context, IStorageService storageService, IConfiguration config)
        {
            _context = context;
            _storageService = storageService;
            _config = config;
        }

        public async Task<List<int>> GetIdBookInGenre(string idBook)
        {
            return await _context.BookInGenres.Where(x => x.IdBook == idBook).Select(x => x.IdGenre).ToListAsync();
        }


        public async Task<bool> PostBookInGenre(string idBook, List<int> idGenres)
        {
            var listGenre = new List<Genre>();

            foreach (var idGenre in idGenres)
            {
                var genre = await _context.Genres.FindAsync(idGenre);
                if(genre is not null)
                    listGenre.Add(genre);
            }

            //Xóa hết thể loại của book
            var oldGenres = _context.BookInGenres.Where(x => x.IdBook == idBook);
            _context.BookInGenres.RemoveRange(oldGenres);
            await _context.SaveChangesAsync();
            foreach (var genre in listGenre)
            {
                var big = await _context.BookInGenres.FindAsync(idBook, genre.Id);
                if (big is not null) continue;
                var newBig = new BookInGenre()
                {
                    IdBook = idBook,
                    IdGenre = genre.Id
                };
                _context.BookInGenres.Add(newBig);
            }

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
