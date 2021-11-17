using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.ViewModels
{
    public class StatisticalShortVM
    {
        public int TotalBookBorrowed { get; set; } = 0;
        public int TotalBookBorrowing { get; set; } = 0;
        public int TotalBookReturn { get; set; } = 0;
        public List<BookVM> TopBooks { get; set; }
        public List<BookVM> ReturnNotEnoughBooks { get; set; }
        public List<LibraryCard> TopLibraryCards { get; set; }
        public List<LibraryCard> CradReturnLate { get; set; }

    }
}
