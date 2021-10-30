using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.ViewModels
{
    public class BookInBorrowVM
    {
        public DateTime TimeBorrowed { get; set; }
        public DateTime TimeReturn { get; set; }
        public DateTime TimeRealReturn { get; set; }
        public int AmountBorrowed { get; set; } = 0;
        public string BorrowStatus { get; set; } = "";
        public string ReturnStatus { get; set; } = "";


        public int AmountReturn { get; set; }



        /* Khóa */
        public string IdBook { get; set; }
        public Book Book { get; set; }

        public Guid IdBorrow { get; set; }
        public Borrow Borrow { get; set; }
        /* Khóa */
    }
}
