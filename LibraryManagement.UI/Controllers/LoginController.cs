using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Entities;
using Library.Library.Entities.Requests;
using LibraryManagement.UI.Constants;
using LibraryManagement.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.UI.Controllers
{
    public class LoginController : Controller
    {


        private readonly UserService _user;
        private readonly IConfiguration _configuration;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public LoginController(UserService user, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _user = user;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private SystemConstants scs = new SystemConstants();

        [Authorize]
        [HttpGet]
        public IActionResult CheckToken()
        {
            return Ok("Token đúng");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var a = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //await HttpContext.AuthenticateAsync();
            
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [AllowAnonymous]
        public IActionResult Index(string returnUrl)
        {
            var login = new LoginRequest();
            login.ReturnUrl = returnUrl;
            return View(login);
        }
        //Login/
        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            //var token = await _user.Authenticate(request);
            var user = await _userManager.FindByNameAsync(request.Username);
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai");
                return View(request);
            }

            //if (string.IsNullOrEmpty(token))
            //{
            //    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai");
            //    return View(request);
            //}

            //var userPrincipal = this.ValidateToken(token);

            //var authProperties = new AuthenticationProperties
            //{
            //    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
            //    IsPersistent = request.RememberMe //xác thực mỗi lần mở lại browser
            //};
            //HttpContext.Session.SetString(scs.Token, token);
            //await HttpContext.SignInAsync(
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    userPrincipal,
            //    authProperties);

            //return RedirectToAction("Index", "Home");
            return Redirect(request.ReturnUrl ?? "/");
        }

        //private ClaimsPrincipal ValidateToken(string jwtToken)
        //{
        //    IdentityModelEventSource.ShowPII = true;


        //    var validationParameters = new TokenValidationParameters
        //    {
        //        ValidateLifetime = true,
        //        ValidIssuer = _configuration["Tokens:Issuer"],
        //        ValidAudience = _configuration["Tokens:Issuer"],
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]))
        //    };

        //    var principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);

        //    return principal;
        //}

    }
}
