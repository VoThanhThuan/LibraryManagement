﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Library.Data.Configurations
{
    public class BookInGenreConfiguration : IEntityTypeConfiguration<BookInGenre>
    {
        public void Configure(EntityTypeBuilder<BookInGenre> builder)
        {
            builder.ToTable("BookInGenre");
            builder.HasKey(x => new {x.IdBook, x.IdGenre});

            builder.HasOne(x => x.Book).WithMany(x => x.BookInGenres)
                .HasForeignKey(x => x.IdBook);
            builder.HasOne(x => x.Genre).WithMany(x => x.BookInGenres)
                .HasForeignKey(x => x.IdGenre);

        }
    }
}
