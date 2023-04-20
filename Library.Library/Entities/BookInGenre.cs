using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Library.Entities
{
    public class BookInGenre
    {
        public string IdBook { get; set; }
        public virtual  Book Book { get; set; }

        public int IdGenre { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
