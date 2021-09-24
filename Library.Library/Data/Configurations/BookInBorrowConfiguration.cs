using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Library.Data.Configurations
{
    public class BookInBorrowConfiguration : IEntityTypeConfiguration<BookInBorrow>
    {
        public void Configure(EntityTypeBuilder<BookInBorrow> builder)
        {
            builder.ToTable("BookInBorrow");
            builder.HasKey(x => new {x.IdBook, x.IdBorrow});
            builder.Property(x => x.TimeBorrowed).HasDefaultValueSql("GETDATE()");
        }
    }
}
