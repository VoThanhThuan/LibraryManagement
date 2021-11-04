using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Data.Configurations;
using Library.Library.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Library.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new BorrowConfiguration());
            modelBuilder.ApplyConfiguration(new BookInBorrowConfiguration());
            
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new BookInGenreConfiguration());
            
            modelBuilder.ApplyConfiguration(new LibraryCardConfiguration());
            modelBuilder.ApplyConfiguration(new LibraryCodeConfiguration());

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());




            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaim");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRole").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogin").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaim");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserToken").HasKey(x => x.UserId);
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<BookInBorrow> BookInBorrows { get; set; }
        public DbSet<BookInGenre> BookInGenres { get; set; }
        public DbSet<LibraryCard> LibraryCards { get; set; }
        public DbSet<LibraryCode> LibraryCodes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<IdentityRoleClaim<Guid>> RoleClaims { get; set; }
        public DbSet<IdentityUserRole<Guid>> AppUserRole { get; set; }

    }
}
