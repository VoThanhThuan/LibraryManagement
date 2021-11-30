using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Entities.ViewModels;

namespace Library.Library.Entities
{
    public class BookInBorrow
    {
        public BookInBorrowVM ToViewModel()
        {
            return new()
            {
                TimeBorrowed = TimeBorrowed,
                TimeReturn = TimeReturn,
                TimeRealReturn = TimeRealReturn,
                AmountBorrowed = AmountBorrowed,
                AmountReturn = AmountReturn,
                BorrowStatus = BorrowStatus,
                ReturnStatus = ReturnStatus,
                IdBook = IdBook,
                IdBorrow = IdBorrow,
                Book = Book,
                Borrow = Borrow
            };
        }

        public DateTime TimeBorrowed { get; set; }
        public DateTime TimeReturn { get; set; }
        public DateTime? TimeRealReturn { get; set; } = null;
        public DateTime? TimeMissing { get; set; } = null;
        public int AmountBorrowed { get; set; } = 0;
        public int AmountReturn { get; set; } = 0;
        public int AmountMissing { get; set; } = 0;
        public string BorrowStatus { get; set; } = "";
        public string ReturnStatus { get; set; } = "";
        

        /* Khóa */
        public string IdBook { get; set; }
        public virtual Book Book { get; set; }

        public Guid IdBorrow { get; set; }
        public virtual Borrow Borrow { get; set; }
        /* Khóa */

    }
}
