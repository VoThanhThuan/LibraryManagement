﻿using Library.Library.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities.ViewModels
{
    public class LibraryCardVM
    {
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

    }
}
