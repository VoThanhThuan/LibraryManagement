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
    public class BorrowConfiguration : IEntityTypeConfiguration<Borrow>
    {
        public void Configure(EntityTypeBuilder<Borrow> builder)
        {
            builder.ToTable("Borrows");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DateBorrow).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.LibraryCard).WithMany(x => x.Borrows)
                .HasForeignKey(x => x.IdCard);
        }
    }
}
