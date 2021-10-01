using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Enums;
using Microsoft.AspNetCore.Http;

namespace Library.Library.Entities.Requests
{
    public class BookRequest
    {
        public Book ToBook()
        {
            return new()
            {
                Id = Id,
                Name = Name,
                PublishingCompany = PublishingCompany,
                PublicationDate = PublicationDate,
                Author = Author,
                Amount = Amount,
                PageNumber = PageNumber,
                DateCanBorrow = DateCanBorrow,
                Rank = Rank,
                IdLibraryCode = IdLibraryCode
            };
        }

        [Display(Name = "Mã Sách")]
        [Required]
        public string Id { get; set; } = ""; //max 8

        public IFormFile Thumbnail { get; set; }

        [Display(Name = "Tên Sách")]
        [Required]
        public string Name { get; set; } = "";

        [Display(Name = "Nhà Xuất Bản")]
        public string PublishingCompany { get; set; } = "";

        [Display(Name = "Ngày Xuất Bản")]
        [DataType(dataType: DataType.Date)]
        public DateTime PublicationDate { get; set; } = DateTime.Now;

        [Display(Name = "Tác Giả")]
        public string Author { get; set; } = "";

        [Range(0, 1000, ErrorMessage = "Số lượng không được nhỏ hơn 0")]
        public int Amount { get; set; } = 0;

        [Range(0, 1000, ErrorMessage = "Số trang không được nhỏ hơn 1")]
        public int PageNumber { get; set; } = 0;

        [Display(Name = "Số Ngày Có Thê Mượn")]
        [Required]
        public int DateCanBorrow { get; set; } = 0;

        [Display(Name = "Hạng")]
        [Required]
        public RankLibrary Rank { get; set; }

        //Khóa ngoại
        [Display(Name = "Mã Thư Viện")]
        [Required]
        public string IdLibraryCode { get; set; } = "";

    }
}
