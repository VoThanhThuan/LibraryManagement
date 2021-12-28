using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Library.Entities.Requests;
using LibraryManagement.UI.Services;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserService _user;
        private readonly RoleService _role;

        public UsersController(UserService user, RoleService role)
        {
            _user = user;
            _role = role;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var user = await _user.GetUsers();
            return View(user);
        }

        // GET: Users
        public async Task<IActionResult> Search(string content, int take)
        {
            var user = await _user.SearchUser(content, take);

            ViewData["Content Search"] = content;
            return View("Index", user);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) {
                TempData["error"] = "id không dược null";
                return RedirectToAction(nameof(Index));
            }

            var user = await _user.GetUser(id);

            if (user == null) {
                TempData["error"] = "Không tìm thấy người dùng";
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Roles"] = await _role.GetRoles();

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRequest user)
        {
            ViewData["Roles"] = await _role.GetRoles();

            if (ModelState.IsValid) {
                var result = await _user.PostUser(user);
                if (result.apiResult == 200)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("mess", result.mess);
                return View(user);
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["Roles"] = await _role.GetRoles();

            if (id == Guid.Empty) {
                TempData["error"] = "Không tìm thầy User";
                return Redirect("/Users");
            }

            //var user = await _context.Users.FindAsync(id);
            var user = await _user.GetUser(id);
            //var roles = await _role.GetRoles();

            if (user == null) {
                TempData["error"] = "Không tìm thấy người dùng";
                return RedirectToAction(nameof(Index));
            }

            return View(user.ToRequest());
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserRequest user)
        {
            if (id != user.Id) {
                TempData["error"] = "Không tìm thầy User";
                return Redirect("/Users");
            }
            ViewData["Roles"] = await _role.GetRoles();

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid) {
                var result = await _user.PutUser(id, user);

                if (!result) {
                    TempData["error"] = "Chỉnh sửa thất bại";
                    return View(user); ;

                }
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _user.GetUser(id);
            return View(user);
        }
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _user.DeleteUser(id);
            if (result == 200)
                return RedirectToAction(nameof(Index));
            TempData["error"] = "Xóa thất bại";
            return Redirect("/Users");
        }

    }
}
