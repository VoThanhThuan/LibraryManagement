using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities
{
    public class BookInBorrow
    {
        public DateTime TimeBorrowed { get; set; }
        public DateTime TimeReturn { get; set; }
        public DateTime TimeRealReturn { get; set; }
        public int AmountBorrowed { get; set; } = 0;
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
