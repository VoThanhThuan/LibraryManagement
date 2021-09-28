using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.Requests
{
    public class UserRequest
    {
        public User ToUser()
        {
            return new User()
            {
                Id = Id,
                Nickname = Nickname,
                Dob = Dob,
                sex = sex,
                Address = Address,
                Email = Email,
                PhoneNumber = PhoneNumber,
                UserName = Username,
                PasswordHash = Password
            };
        }
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nickname là bắt buộc")]
        public string Nickname { get; set; } = "";
        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }
        public IFormFile Avatar { get; set; }
        [Required]
        public bool sex { get; set; } = true;
        public string Address { get; set; } = "";

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = "";

        public string PhoneNumber { get; set; } = "";
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không chính xác")]
        public string ConfirmPassword { get; set; } = "";
        public Guid IdRole { get; set; }
    }
}
