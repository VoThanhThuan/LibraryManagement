using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities
{
    public class Borrow
    {
        public Guid Id { get; set; }
        public DateTime DateBorrow { get; set; } = DateTime.Now;
        public string Note { get; set; } = "";

        //Khóa ngoại
        public Guid IdUser { get; set; }
        public virtual User User { get; set; }

        public Guid IdCard { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
    }
}
