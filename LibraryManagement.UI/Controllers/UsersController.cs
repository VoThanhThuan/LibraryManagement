using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.Requests;
using LibraryManagement.UI.Services;

namespace LibraryManagement.UI.Controllers
{
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

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var user = await _user.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRequest user)
        {
            if (ModelState.IsValid)
            {
                var result =  await _user.PostUser(user);
                if (result.apiResult == 200)
                    return RedirectToAction(nameof(Index));
                else
                    return Conflict();
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            //var user = await _context.Users.FindAsync(id);
            var user = await _user.GetUser(id);
            var roles = await _role.GetRoles();
            if (user == null)
            {
                return NotFound();
            }

            return View((user.ToRequest(), roles));
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserRequest user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                var result = await _user.PutUser(id, user);

                if (!result)
                    return Conflict();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _user.DeleteUser(id);
            if(result == 200)
                return RedirectToAction(nameof(Index));
            return Conflict($"Lỗi xóa {result}");
        }

    }
}
