﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Entities.Requests;

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
                UserName = UserName

            };
        }

        public Guid Id { get; set; }
        public DateTime DateBorrow { get; set; } = DateTime.Now;
        public string Note { get; set; } = "";

        //Dữ liệu bindcode không cần khóa
        public Guid IdUser { get; set; }
        public string UserName { get; set; }

        //Khóa ngoại

        public Guid IdCard { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
        public virtual List<BookInBorrow> BookInBorrows { get; set; }
    }
}
