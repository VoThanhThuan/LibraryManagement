using System;
using System.Collections.Generic;
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

        //Khóa ngoại
        public LibraryCard LibraryCard { get; set; } = new();
        public List<Book> Books { get; set; } = new();
    }
}
