using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Entities.Requests;

namespace Library.Library.Entities.ViewModels
{
    public class UserVM
    {
        public UserRequest ToRequest()
        {
            return new UserRequest()
            {
                Id = Id,
                Nickname = Nickname,
                Dob = Dob,
                sex = sex,
                Address = Address,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Username = Username,
                Password = Password,
                IdRole = IdRole
            };
        }
        public Guid Id { get; set; }
        public string Nickname { get; set; } = "";
        public string Avatar { get; set; } = "";
        public DateTime? Dob { get; set; }
        public bool sex { get; set; } = true;
        public string Address { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Username { get; set; }
        public string Password { get; set; }

        public string RoleName { get; set; }
        public Guid IdRole { get; set; }


    }
}
