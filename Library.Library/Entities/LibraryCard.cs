using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities
{
    public class LibraryCard
    {
        public Guid Id { get; set; }
        public string MSSV { get; set; } = "";
        public string Class { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public int Karma { get; set; } = 0;
        public bool IsClock { get; set; } = false;
        public int Rank { get; set; } = 0;
        public int Exp { get; set; } = 0; //experience
    }
}
