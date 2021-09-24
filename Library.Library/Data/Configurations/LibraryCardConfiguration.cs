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
    public class LibraryCardConfiguration : IEntityTypeConfiguration<LibraryCard>
    {
        public void Configure(EntityTypeBuilder<LibraryCard> builder)
        {
            builder.ToTable("LibraryCards");
            builder.Property(x => x.MSSV).HasMaxLength(10);
            builder.Property(x => x.Class).HasMaxLength(255);
            builder.Property(x => x.Karma).HasDefaultValue(0);
            builder.Property(x => x.Rank).HasDefaultValue(0);
            builder.Property(x => x.IsClock).HasDefaultValue(false);
        }
    }
}
