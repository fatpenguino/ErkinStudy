using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRoleController(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: UserRole
        public async Task<IActionResult> Index()
        {
            var userRoles = _context.UserRoles.ToList();
            var resultList = new List<UserRoleViewModel>();
            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByIdAsync(userRole.RoleId.ToString());
                var user = await _userManager.FindByIdAsync(userRole.UserId.ToString());
                var item = new UserRoleViewModel
                {
                    UserId = userRole.UserId,
                    RoleId = userRole.RoleId,
                    Username = $"{user.LastName} {user.FirstName}",
                    RoleName = role.Name
                };
                resultList.Add(item);
            }
            return View(resultList);
        }

        // GET: UserRole/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string email, long roleId)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userRole = new ApplicationUserRole {RoleId = roleId, UserId = user.Id};
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: UserRole/Delete/5
        public async Task<IActionResult> Delete(long userId, long roleId)
        {
            var applicationUserRole = await _context.UserRoles
                .FirstOrDefaultAsync(m => m.UserId == userId && m.RoleId == roleId);
            if (applicationUserRole == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(applicationUserRole.RoleId.ToString());
            var user = await _userManager.FindByIdAsync(applicationUserRole.UserId.ToString());
            var item = new UserRoleViewModel
            {
                UserId = applicationUserRole.UserId,
                RoleId = applicationUserRole.RoleId,
                Username = $"{user.LastName} {user.FirstName}",
                RoleName = role.Name
            };
            return View(item);
        }

        // POST: UserRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long userId, long roleId)
        {
            var applicationUserRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.RoleId == roleId && x.UserId == userId);
            _context.UserRoles.Remove(applicationUserRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
