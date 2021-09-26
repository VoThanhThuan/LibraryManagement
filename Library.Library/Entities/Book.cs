using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Enums;

namespace Library.Library.Entities
{
    public class Book
    {
        public string Id { get; set; } = ""; //max 8
        public string Thumbnail { get; set; } = "";
        public string Name { get; set; } = "";
        public string PublishingCompany { get; set; } = "";
        public DateTime PublicationDate { get; set; } = DateTime.Now;
        public string Author { get; set; } = "";
        public int Amount { get; set; } = 0;
        public int PageNumber { get; set; } = 0;

        public DateTime DateCanBorrow { get; set; }
        public RankLibrary Rank { get; set; }

        //Khóa ngoại
        public string IdLibraryCode { get; set; } = "";
        public virtual LibraryCode LibraryCode { get; set; }
        public virtual List<BookInBorrow> BookInBorrows { get; set; }
        public virtual List<BookInGenre> BookInGenres { get; set; }

    }
}
