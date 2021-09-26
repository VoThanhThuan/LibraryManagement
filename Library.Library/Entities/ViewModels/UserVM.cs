using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.ViewModels
{
    public class UserVM
    {
        public string Nickname { get; set; } = "";
        public string Avatar { get; set; } = "";
        public DateTime? Dob { get; set; }
        public bool sex { get; set; } = true;
        public string Address { get; set; } = "";

    }
}
