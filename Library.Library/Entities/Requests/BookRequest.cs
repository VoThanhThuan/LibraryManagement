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
            return new() {
                Id = Id,
                Name = Name,
                PublishingCompany = PublishingCompany,
                PublicationDate = PublicationDate,
                Author = Author,
                Amount = Amount,
                PageNumber = PageNumber,
                DateCanBorrow = DateCanBorrow,
                Rank = Rank,
                StatusBook = StatusBook,
                IdLibraryCode = IdLibraryCode
            };
        }

        [Display(Name = "Mã Sách")]
        [Required(ErrorMessage = "Mã sách là bắt buộc")]
        public string Id { get; set; } = ""; //max 8

        [Required(ErrorMessage = "Ảnh bìa của sách là bắt buộc")]
        public IFormFile Thumbnail { get; set; }

        [Display(Name = "Tên Sách")]
        [Required(ErrorMessage = "tên sách là bắt buộc")]
        public string Name { get; set; } = "";

        [Display(Name = "Nhà Xuất Bản")]
        public string PublishingCompany { get; set; } = "";

        [Display(Name = "Ngày Xuất Bản")]
        public DateTime PublicationDate { get; set; } = DateTime.Now;

        [Display(Name = "Tác Giả")]
        public string Author { get; set; } = "";

        [Range(0, 1000, ErrorMessage = "Số lượng không được nhỏ hơn 0 và lớn hơn 1000")]
        [Required(ErrorMessage = "số lượng sách là bắt buộc")]
        public int Amount { get; set; } = 0;

        [Range(0, 1000, ErrorMessage = "Số trang không được nhỏ hơn 1 và lớn hơn 1000")]
        [Required(ErrorMessage = "số trang sách là bắt buộc")]
        public int PageNumber { get; set; } = 0;

        [Display(Name = "Số Ngày Có Thê Mượn")]
        [Required(ErrorMessage = "ngày có thể mượn sách là bắt buộc")]
        public int DateCanBorrow { get; set; } = 0;

        [Display(Name = "Hạng")]
        [Required(ErrorMessage = "hạng sách là bắt buộc")]
        public RankLibrary Rank { get; set; } = RankLibrary.Beginner;

        [Display(Name = "Tình trạng sách")]
        [Required(ErrorMessage = "tình trạng  sách là bắt buộc")]
        public StatusBook StatusBook { get; set; } = StatusBook.CanBorrow;

        [Display(Name = "Sô Lần Mượn")]
        public int TotalBorrow { get; set; } = 0;

        //Khóa ngoại
        [Display(Name = "Mã Thư Viện")]
        [Required(ErrorMessage = "mã thư viện là bắt buộc")]
        public string IdLibraryCode { get; set; } = "";

        [Required(ErrorMessage = "thể loại là bắt buộc")]
        public List<int> idGenres { get; set; }

    }
}
