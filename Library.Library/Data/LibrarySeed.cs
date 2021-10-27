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
                user0.PasswordHash = _passwordHasher.HashPassword(user0, "admin");
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
                user1.PasswordHash = _passwordHasher.HashPassword(user1, "admin");
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
                user2.PasswordHash = _passwordHasher.HashPassword(user2, "admin");
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
                user3.PasswordHash = _passwordHasher.HashPassword(user3, "admin");
                context.Users.Add(user3);
                await context.SaveChangesAsync();

                //Quyền hạng

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
                    Description =
                        "Tiểu thuyết là một thể loại văn xuôi có hư cấu, thông qua nhân vật, hoàn cảnh, sự việc để phản ánh bức tranh xã hội rộng lớn và những vấn đề của cuộc sống con người, biểu hiện tính chất tường thuật, tính chất kể chuyện bằng ngôn ngữ văn xuôi theo những chủ đề xác định."
                };
                context.Genres.Add(manhua);
            }

            if (!context.LibraryCodes.Any())
            {
                var vh = new LibraryCode()
                {
                    Id = "VHHD0001",
                    Name = "Văn Học Hiện Đại",
                    Abbreviation = "VHHD",
                    Description =
                        "Văn học hậu hiện đại là trào lưu văn học xuất hiện sau Chiến tranh thế giới thứ hai tại xã hội Tây phương, đỉnh cao là vào những năm 70, 80, với hàng loạt các kỹ thuật sáng tác và tư tưởng văn nghệ mới để phản ứng lại các quy chuẩn của văn học hiện đại, trong khi đó cũng phát triển thêm các kỹ thuật và giả định cơ bản của văn học hiện đại."
                };
                context.LibraryCodes.Add(vh);
                var tith = new LibraryCode()
                {
                    Id = "TITH0001",
                    Name = "Tiểu Thuyết",
                    Abbreviation = "TT",
                    Description =
                        "Tiểu thuyết là một thể loại văn xuôi có hư cấu, thông qua nhân vật, hoàn cảnh, sự việc để phản ánh bức tranh xã hội rộng lớn và những vấn đề của cuộc sống con người, biểu hiện tính chất tường thuật, tính chất kể chuyện bằng ngôn ngữ văn xuôi theo những chủ đề xác định."
                };
                context.LibraryCodes.Add(tith);

                var trtr = new LibraryCode()
                {
                    Id = "TRTR0001",
                    Name = "Truyện Tranh",
                    Abbreviation = "TT",
                    Description =
                        "Ghé thăm website truyện tranh của [Võ Thành Thuận] để đọc những trang truyện tuyển chọn hay nhất."
                };
                context.LibraryCodes.Add(trtr);
            }

            if (!context.LibraryCards.Any())
            {
                context.LibraryCards.Add(new LibraryCard()
                {
                    Id = new Guid(),
                    Class = "DH19PM",
                    Exp = 1,
                    IsClock = false,
                    MSSV = "DPM1851XX",
                    PhoneNumber = "0123456789",
                    Rank = 0
                });
            }

            await context.SaveChangesAsync();

        }

    }

}


