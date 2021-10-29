using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.Requests
{
    public class BorrowVM
    {
        public Guid Id { get; set; }
        public DateTime DateBorrow { get; set; } = DateTime.Now;
        public string Note { get; set; } = "";

        //Dữ liệu bindcode không cần khóa
        public Guid IdUser { get; set; }
        public string UserName { get; set; }

        [Display(Name = "Số lượng mượn")] public int AmountBorrow { get; set; } = 0;

        [Display(Name = "Số lượng đã trả")] public int AmountReturned { get; set; } = 0;


        //Khóa ngoại
        public LibraryCard LibraryCard { get; set; } = new();
        public List<Book> Books { get; set; } = new();
    }
}
