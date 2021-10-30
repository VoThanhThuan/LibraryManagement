using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.Requests
{
    public class ReturnBookRequest
    {
        public Guid IdCard { get; set; }
        public Guid IdBorrow { get; set; }
        public string IdBook { get; set; }
        public int AmountBorrowed { get; set; }
        public int AmountReturn { get; set; }
    }
}
