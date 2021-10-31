using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.Requests;
using Library.Library.Entities.ViewModels;
using LibraryManagement.UI.Models.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.UI.Services
{
    public class UserService
    {
        private readonly LibraryDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        private readonly UserManager<User> _userManager; //Thư viện quản lý user
        private readonly RoleManager<Role> _roleManager; //Thư viện quản lý role
        private readonly SignInManager<User> _signInManager; //Thư viên đăng nhập
        private readonly IConfiguration _config; //lấy config từ appsetting.config

        public UserService(LibraryDbContext context,
           
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IStorageService storageService,
            IConfiguration config)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _storageService = storageService;
            _config = config;
        }
        public async Task<List<UserVM>> GetUsers()
        {
            var users = await _context.Users.Select(x => x.ToViewModel()).ToListAsync();

            return users;
        }

        public async Task<UserVM> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            //var role = _context.AppUserRole.FirstOrDefault(x => x.UserId == user.Id);
            return await user.ToViewModel(_userManager);
        }

        public async Task<bool> PutUser(Guid id, UserRequest request)
        {
            var text = new TextService();
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                return false;
            user.Nickname = string.IsNullOrEmpty(text.RemoveSpaces(request.Nickname)) == true ? user.Nickname : request.Nickname;
            user.Dob = request.Dob ?? user.Dob;
            user.sex = request.sex != user.sex ? request.sex : user.sex;
            user.Address = string.IsNullOrEmpty(text.RemoveSpaces(request.Address)) == true ? user.Address : request.Address;
            user.Email = string.IsNullOrEmpty(text.RemoveSpaces(request.Email)) == true ? user.Email : request.Email;
            user.PhoneNumber = string.IsNullOrEmpty(text.RemoveSpaces(request.PhoneNumber)) == true ? user.PhoneNumber : request.PhoneNumber;
            user.UserName = string.IsNullOrEmpty(text.RemoveSpaces(request.Username)) == true ? user.UserName : request.Username;

            if (!string.IsNullOrEmpty(text.RemoveSpaces(request.Password)))
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            if (request.Avatar != null)
            {
                await DeleteFile(user.Avatar);
                user.Avatar = await SaveFile(request.Avatar);
            }

            //_context.Entry(request.ToUser()).State = EntityState.Modified;

            //Role assign
            var checkRole = _context.AppUserRole.FirstOrDefault(x => x.UserId == user.Id);
            if (checkRole.RoleId == request.IdRole)
            {
                var roles = await _context.Roles.Select(x => x).ToListAsync();
                var userRole = await _context.AppUserRole.FindAsync(id, request.IdRole);
                if (userRole == null)
                {
                    foreach (var role in roles)
                    {
                        if (await _userManager.IsInRoleAsync(user, role.Name) == true)
                        {
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                    }

                    var roleNew = await _context.Roles.FindAsync(request.IdRole);

                    await _userManager.AddToRoleAsync(user, roleNew.Name);
                }

            }

            try
            {
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return false;
                throw;
            }

            return true;
        }

        public async Task<(int apiResult, string mess, UserVM user)> PostUser(UserRequest request)
        {
            var checkUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);
            if (checkUser != null)
                return (StatusCodes.Status409Conflict, "Username đã tồn tại", null);
            var user = request.ToUser();
            user.Id = Guid.NewGuid();
            user.Avatar = await SaveFile(request.Avatar);
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return (StatusCodes.Status409Conflict, "Tạo tài khoản thất bại", null);
            }

            //Role assign
            var role = await _context.Roles.FindAsync(request.IdRole);
            if (request.IdRole == Guid.Empty || role == null)
            {
                role = await _roleManager.FindByNameAsync("Librarian");
            }

            await _userManager.AddToRoleAsync(user, role.Name);

            return (StatusCodes.Status200OK, "Ok", user.ToViewModel());
        }

        public async Task<(int apiResult, string mess, UserVM user)> Register(UserRequest request)
        {
            var checkUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);
            if (checkUser != null)
                return (StatusCodes.Status409Conflict, "Username đã tồn tại", null);
            var user = request.ToUser();
            user.Id = Guid.NewGuid();
            user.Avatar = await SaveFile(request.Avatar);
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return (StatusCodes.Status409Conflict, "Tạo tài khoản thất bại", null);
            }

            //Role assign
            var role = await _roleManager.FindByNameAsync("Guest");

            await _userManager.AddToRoleAsync(user, role.Name);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (StatusCodes.Status200OK, "Ok", user.ToViewModel());
        }

        public async Task<int> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return StatusCodes.Status404NotFound;
            }

            var result = await DeleteFile(user.Avatar);
            if (result == 500)
                return StatusCodes.Status500InternalServerError;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();


            return StatusCodes.Status200OK;
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            return await _storageService.SaveFileAsync(file, @"avatar");
        }
        private async Task<int> DeleteFile(string fileName)
        {
            return await _storageService.DeleteFileAsync(fileName);
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null) return null;

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            //var checkpass = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            //if (checkpass != PasswordVerificationResult.Success)
            //{
            //    return null;
            //}

            //await _signInManager.SignInAsync(user, isPersistent: true);
            //if (!result.Succeeded)
            //    return null;

            var roles = await _userManager.GetRolesAsync(user); //lấy quyền người dùng
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, string.IsNullOrEmpty(user.Email) ? "" : user.Email),
                new Claim(ClaimTypes.GivenName, user.Nickname),
                new Claim(ClaimTypes.Role, string.Join(";", roles)),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            //<>Mã hóa SymmetricS
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //<> Tạo Token
            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
            //</>

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string DecryptString(string cipherText)
        {
            //<>Mã hóa SymmetricS
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return "";
        }

    }
}
