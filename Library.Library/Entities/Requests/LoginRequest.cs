using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Tên Đăng Nhập Không Được Bỏ Trống")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mật Khẩu Không Được Bỏ Trống")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }


    }
}
