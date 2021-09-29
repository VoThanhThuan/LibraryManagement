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
        private readonly LibraryDbContext _context;
        private readonly UserService _user;
        private readonly RoleService _role;

        public UsersController(LibraryDbContext context, UserService user, RoleService role)
        {
            _context = context;
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

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
