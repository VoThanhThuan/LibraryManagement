using Library.Library.Entities;
using Library.Library.Entities.Requests;
using LibraryManagement.UI.Constants;
using LibraryManagement.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace LibraryManagement.UI.Controllers
{
    public class LoginController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
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
