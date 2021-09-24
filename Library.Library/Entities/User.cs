using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Library.Library.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Nickname { get; set; } = "";
        public string Avatar { get; set; } = "";
        public DateTime? Dob { get; set; }
        public bool sex { get; set; } = true;
        public string Address { get; set; } = "";
        //Khóa ngoại

    }
}
