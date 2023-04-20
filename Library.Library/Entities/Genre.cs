using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities
{
    public class Genre
    {
        public int Id { get; set; } //Tự tăng
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        /* khóa ngoại */
        public virtual List<BookInGenre> BookInGenres { get; set; }
    }
}
