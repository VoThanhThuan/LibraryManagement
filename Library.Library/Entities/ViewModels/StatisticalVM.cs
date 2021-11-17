using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.ViewModels
{
    public class StatisticalVM
    {
        public List<BookVM> BooksBorrowed { get; set; }
        public List<BookVM> BooksBorrowing { get; set; }
        public List<BookVM> BooksReturn { get; set; }
        public List<BookVM> BooksMissing { get; set; }
        public List<BookVM> TopBooks { get; set; }
        public List<BookVM> ReturnNotEnoughBooks { get; set; }
        public List<LibraryCard> TopLibraryCards { get; set; }
        public List<LibraryCard> CardReturnLate { get; set; }
        public List<LibraryCard> CardsLock { get; set; }
    }
}
