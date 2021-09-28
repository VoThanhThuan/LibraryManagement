using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Library.Data
{
    public class LibrarySeed
    {
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public async Task SeedAsync(LibraryDbContext context, ILogger<LibrarySeed> logger)
        {
            var ra = Guid.NewGuid();
            var rl = Guid.NewGuid();
            if (!context.Roles.Any())
            {
                var admin = new Role()
                {
                    Id = ra,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                };
                context.Roles.Add(admin);

                var librarian = new Role()
                {
                    Id = rl,
                    Name = "Librarian",
                    NormalizedName = "LIBRARIAN"
                };
                context.Roles.Add(librarian);
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                //Tài khoản
                var id0 = Guid.NewGuid();
                var user0 = new User()
                {
                    Id = id0,
                    Nickname = "Web Master",
                    Email = "master@thuan.com",
                    NormalizedEmail = "THUAN@THUAN.COM",
                    PhoneNumber = "0123456789",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                user0.PasswordHash = _passwordHasher.HashPassword(user0, "Admin123$");
                context.Users.Add(user0);

                var idt = Guid.NewGuid();
                var user1 = new User()
                {
                    Id = idt,
                    Nickname = "Thu.An",
                    Email = "thuan@thuan.com",
                    NormalizedEmail = "THUAN@THUAN.COM",
                    PhoneNumber = "0123456789",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                user1.PasswordHash = _passwordHasher.HashPassword(user1, "Admin123$");
                context.Users.Add(user1);

                var ida = Guid.NewGuid();
                var user2 = new User()
                {
                    Id = ida,
                    Nickname = "Anvy",
                    Email = "anvy@thuan.com",
                    NormalizedEmail = "ANVY@THUAN.COM",
                    PhoneNumber = "0123456789",
                    UserName = "Librarian",
                    NormalizedUserName = "LIBRARIAN",
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                user2.PasswordHash = _passwordHasher.HashPassword(user2, "Admin123$");
                context.Users.Add(user2);

                var ids = Guid.NewGuid();
                var user3 = new User()
                {
                    Id = ids,
                    Nickname = "noS",
                    Email = "nos@thuan.com",
                    NormalizedEmail = "NOS@THUAN.COM",
                    PhoneNumber = "0123456789",
                    UserName = "Librarian",
                    NormalizedUserName = "LIBRARIAN",
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                user3.PasswordHash = _passwordHasher.HashPassword(user3, "Admin123$");
                context.Users.Add(user3);
                await context.SaveChangesAsync();

                //Quyền hạng
                //var modelBuilder = new ModelBuilder();
                //modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
                //{
                //    RoleId = ra,
                //    UserId = id0,
                //});
                //modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
                //{
                //    RoleId = ra,
                //    UserId = idt,
                //});
                //modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
                //{
                //    RoleId = rl,
                //    UserId = ida,
                //});
                //modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
                //{
                //    RoleId = rl,
                //    UserId = ids,
                //});

                context.Add(new IdentityUserRole<Guid>
                {
                    RoleId = ra,
                    UserId = id0,
                });
                context.Add(new IdentityUserRole<Guid>
                {
                    RoleId = ra,
                    UserId = idt,
                });
                context.Add(new IdentityUserRole<Guid>
                {
                    RoleId = rl,
                    UserId = ida,
                });
                context.Add(new IdentityUserRole<Guid>
                {
                    RoleId = rl,
                    UserId = ids,
                });
                await context.SaveChangesAsync();
            }

            if (!context.Genres.Any())
            {
                var manga = new Genre()
                {
                    Name = "Văn Học",
                    Description = "Văn học theo cách nói chung nhất, là bất kỳ tác phẩm nào bằng văn bản. " +
                                  "Hiểu theo nghĩa hẹp hơn, thì văn học là dạng văn bản được coi là một hình thức nghệ thuật, hoặc bất kỳ một bài viết nào được coi là có giá trị nghệ thuật hoặc trí tuệ, thường là do cách thức triển khai ngôn ngữ theo những cách khác với cách sử dụng bình thường."
                };
                context.Genres.Add(manga);

                var manhwa = new Genre()
                {
                    Name = "Toán Học",
                    Description = "Toán học là ngành nghiên cứu trừu tượng về những chủ đề như: " +
                                  "lượng (các con số), cấu trúc, không gian, và sự thay đổi." +
                                  " Các nhà toán học và triết học có nhiều quan điểm khác nhau về định nghĩa và phạm vi của toán học."
                };
                context.Genres.Add(manhwa);

                var manhua = new Genre()
                {
                    Name = "Tiểu Thuyết",
                    Description = "Tiểu thuyết là một thể loại văn xuôi có hư cấu, thông qua nhân vật, hoàn cảnh, sự việc để phản ánh bức tranh xã hội rộng lớn và những vấn đề của cuộc sống con người, biểu hiện tính chất tường thuật, tính chất kể chuyện bằng ngôn ngữ văn xuôi theo những chủ đề xác định."
                };
                context.Genres.Add(manhua);

            }

            await context.SaveChangesAsync();

        }
    }
}
