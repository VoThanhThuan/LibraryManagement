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
    public class LibraryCodeConfiguration : IEntityTypeConfiguration<LibraryCode>
    {
        public void Configure(EntityTypeBuilder<LibraryCode> builder)
        {
            builder.ToTable("LibraryCode");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(8);
            builder.Property(x => x.Name).HasMaxLength(255);
            builder.Property(x => x.Abbreviation).HasMaxLength(50);

        }
    }
}
