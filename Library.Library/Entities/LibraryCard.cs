using Library.Library.Entities.ViewModels;
using Library.Library.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Library.Entities
{
    public class LibraryCard
    {
        public LibraryCardVM ToViewModel()
        {
            return new() {
                Id = Id,
                MSSV = MSSV,
                Name = Name,
                Class = Class,
                PhoneNumber = PhoneNumber,
                Karma = Karma,
                IsLock = IsLock,
                Rank = Rank,
                StatusCard = StatusCard,
                Exp = Exp,
                ExpLevelUp = ExpLevelUp,
                Image = Image
            };
        }
        public Guid Id { get; set; }
        [Required]
        public string MSSV { get; set; } = "";
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string Class { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public int Karma { get; set; } = 0;
        public bool IsLock { get; set; } = false;
        public RankLibrary Rank { get; set; } = RankLibrary.Beginner;
        public StatusCard StatusCard { get; set; } = StatusCard.CanBorrow;
        public int Exp { get; set; } = 0; //experience
        public int ExpLevelUp { get; set; } = 30; //Kinh nghiệm cần để lên cấp
        public string Image { get; set; } = ""; //hình thẻ

        /*Khóa ngoại*/
        public virtual List<Borrow> Borrows { get; set; }
    }
}
