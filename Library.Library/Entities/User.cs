using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Library.Library.Entities.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Library.Library.Entities
{
    public class User : IdentityUser<Guid>
    {
        public UserVM ToViewModel()
        {
            return new UserVM()
            {
                Id = Id,
                Nickname = Nickname,
                Avatar = Avatar,
                Dob = Dob,
                sex = sex,
                Address = Address,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Username = UserName,
                Password = PasswordHash
            };
        }

        public async Task<UserVM> ToViewModel(UserManager<User> userManager)
        {
            var user = new UserVM()
            {
                Id = Id,
                Nickname = Nickname,
                Avatar = Avatar,
                Dob = Dob,
                sex = sex,
                Address = Address,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Username = UserName,
                Password = PasswordHash
            };
            user.RoleName = (await userManager.GetRolesAsync(this))[0];
            return user;
        }

        public string Nickname { get; set; } = "";
        public string Avatar { get; set; } = "";

        [Display(Name = "Ngày sinh")]
        public DateTime? Dob { get; set; }
        public bool sex { get; set; } = true;
        public string Address { get; set; } = "";


    }
}
