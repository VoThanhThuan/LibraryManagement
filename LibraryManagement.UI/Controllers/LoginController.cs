using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.Library.Entities.Requests;
using LibraryManagement.UI.Constants;
using LibraryManagement.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.UI.Controllers
{
    public class LoginController : Controller
    {
        public LoginController(UserService user, IConfiguration configuration)
        {
            _user = user;
            _configuration = configuration;
        }

        private readonly UserService _user;
        private readonly IConfiguration _configuration;

        private SystemConstants scs = new SystemConstants();

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CheckToken()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var a = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.AuthenticateAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        //Login/

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid) return View(request);

            var token = await _user.Authenticate(request);

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai");
                return View(request);
            }

            var userPrincipal = this.ValidateToken(token);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = true //xác thực mỗi lần mở lại browser
            };
            HttpContext.Session.SetString(scs.Token, token);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;


            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidIssuer = _configuration["Tokens:Issuer"],
                ValidAudience = _configuration["Tokens:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]))
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);

            return principal;
        }

    }
}
