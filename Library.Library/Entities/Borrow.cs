using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Entities.Requests;
using Library.Library.Enums;

namespace Library.Library.Entities
{
    public class Borrow
    {

        public BorrowVM ToViewModel()
        {
            return new BorrowVM()
            {
                Id = Id,
                DateBorrow = DateBorrow,
                Note = Note,
                IdUser = IdUser,
                UserName = UserName,
                AmountBorrow = AmountBorrow,
                AmountReturned = AmountReturned
            };
        }

        public Guid Id { get; set; }
        public DateTime DateBorrow { get; set; } = DateTime.Now;
        public string Note { get; set; } = "";

        //Dữ liệu bindcode không cần khóa
        public Guid IdUser { get; set; }
        public string UserName { get; set; }

        [Display(Name = "Số lượng mượn")] public int AmountBorrow { get; set; } = 0;

        [Display(Name = "Số lượng đã trả")] public int AmountReturned { get; set; } = 0;
        [Display(Name = "Tình trạng mượn")] public StatusBorrow StatusBorrow { get; set; } = StatusBorrow.Borrowing;

        //Khóa ngoại

        public Guid IdCard { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
        public virtual List<BookInBorrow> BookInBorrows { get; set; }
    }
}
