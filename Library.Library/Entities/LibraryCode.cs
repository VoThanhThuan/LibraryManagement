using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities
{
    public class LibraryCode
    {
        public string Id { get; set; } = ""; //max 8
        public string Name { get; set; } = "";
        public string Abbreviation { get; set; } = ""; //Viết tắt
        public string Description { get; set; } = "";
    }
}
