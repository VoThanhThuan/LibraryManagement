using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Entities.Requests;
using Library.Library.Entities.ViewModels;
using Library.Library.Enums;

namespace Library.Library.Entities
{
    public class Book
    {
        public BookRequest ToRequest()
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
                IdLibraryCode = IdLibraryCode
            };
        }

        public BookVM ToViewModel()
        {
            var bookVM = new BookVM() {
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
                TotalBorrow = TotalBorrow,
                IdLibraryCode = IdLibraryCode,
                Thumbnail = Thumbnail,
            };
            if(LibraryCode != null) {
                bookVM.AbbreviationLibraryCode = LibraryCode.Abbreviation;
            }
            return bookVM;
        }

        public BookVM ToViewModel(BookInBorrowVM bookInBorrow, int statisticalAmount, LibraryCardVM libraryCard=null)
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
                TotalBorrow = TotalBorrow,
                IdLibraryCode = IdLibraryCode,
                Thumbnail = Thumbnail,
                TimeBorrowed = bookInBorrow.TimeBorrowed,
                TimeReturn = bookInBorrow.TimeReturn,
                TimeMissing = bookInBorrow.TimeMissing,
                TimeRealReturn = bookInBorrow.TimeRealReturn,
                StatisticalAmount = statisticalAmount,
                BookInBorrow = bookInBorrow,
                LibraryCard = libraryCard
            };
        }

        [Display(Name = "Mã Sách")]
        [Required(ErrorMessage = "Mã sách là bắt buộc")]
        public string Id { get; set; } = ""; //max 8

        public string Thumbnail { get; set; } = "";

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
        public virtual LibraryCode LibraryCode { get; set; }
        public virtual List<BookInBorrow> BookInBorrows { get; set; }
        public virtual List<BookInGenre> BookInGenres { get; set; }

    }
}
