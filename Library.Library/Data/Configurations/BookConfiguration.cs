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
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(8);
            builder.Property(x => x.PublishingCompany).HasMaxLength(255);
            builder.Property(x => x.Author).HasMaxLength(255);

            builder.HasOne(x => x.LibraryCode).WithMany(x => x.Book)
                .HasForeignKey(x => x.IdLibraryCode);

        }
    }
}
