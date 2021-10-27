using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.Requests
{
    public class BorrowRequest
    {
        public BorrowVM ToViewModel()
        {
            return new BorrowVM()
            {
                Id = Id,
                DateBorrow = DateBorrow,
                Note = Note,
                IdUser = IdUser
            };
        }

        public Borrow ToBorrow()
        {
            return new Borrow()
            {
                Id = Id,
                DateBorrow = DateBorrow,
                Note = Note,
                IdUser = IdUser
            };
        }

        public Guid Id { get; set; }
        public DateTime DateBorrow { get; set; } = DateTime.Now;
        public string Note { get; set; } = "";

        //Dữ liệu bindcode không cần khóa
        public Guid IdUser { get; set; }
        public Guid IdCard { get; set; }

    }
}
