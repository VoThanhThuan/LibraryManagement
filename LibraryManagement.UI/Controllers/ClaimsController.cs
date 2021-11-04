using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Library.Data;
using Library.Library.Entities;
using Library.Library.Entities.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.UI.Controllers
{
    [Route("[controller]")]
    public class ClaimsController : Controller
    {
        private readonly RoleManager<Role> _role;
        private readonly LibraryDbContext _context;

        public ClaimsController(RoleManager<Role> role, LibraryDbContext context)
        {
            _role = role;
            _context = context;
        }

        [HttpGet("Create/{idRole}")]
        public IActionResult Create(Guid idRole)
        {
            return View();
        }

        [HttpPost("Create/{idRole}")]
        public async Task<IActionResult> Create(Guid idRole, RoleClaimVM request)
        {
            var role = await _role.FindByIdAsync(idRole.ToString());

            var claim = new Claim(request.ClaimType, request.ClaimValue);

            await _role.AddClaimAsync(role, claim);

            return View();
            //return Redirect($"/Claims/Create/{idRole}");
        }

        [HttpGet("Edit/{idRole}")]
        public async Task<IActionResult> Edit(Guid idRole)
        {
            var role = await _context.RoleClaims.FirstOrDefaultAsync(x => x.RoleId == idRole);

            var roleVM = new RoleClaimVM()
            {
                ClaimType = role.ClaimType,
                ClaimValue = role.ClaimValue
            };

            return View(roleVM);
            //return Redirect($"/Claims/Create/{idRole}");
        }        
        [HttpPut("Edit/{idRole}")]
        public async Task<IActionResult> Edit(Guid idRole, RoleClaimVM request)
        {
            var role = await _context.RoleClaims.FirstOrDefaultAsync(x => x.RoleId == idRole);

            role.ClaimType = request.ClaimType;
            role.ClaimValue = request.ClaimValue;

            await _context.SaveChangesAsync();

            //return View();
            return Redirect($"/Claims/Edit/{idRole}");
        }

    }
}
