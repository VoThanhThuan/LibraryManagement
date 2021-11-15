using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UI.Services {
    public class LibraryCardService {


        private readonly LibraryDbContext _context;

        public LibraryCardService(LibraryDbContext context) {
            _context = context;
        }

        public async Task<List<LibraryCard>> TopLibraryCards()
        {
            var cards = await _context.LibraryCards.OrderByDescending(x => x.Exp).ToListAsync();
            return cards;
        }
    }
}
