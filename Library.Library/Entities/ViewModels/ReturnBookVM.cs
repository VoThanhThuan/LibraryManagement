using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.ViewModels
{
    public class ReturnBookVM
    {
        public Guid IdCard { get; set; }
        public Guid IdBorrow { get; set; }

        public LibraryCard LibraryCard { get; set; }
        public List<BookInBorrow> BookInBorrows { get; set; }

    }

}
