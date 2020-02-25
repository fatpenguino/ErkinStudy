using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace ErkinStudy.Web.Controllers.Admin
{
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

        // GET: UserRole/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserRole = await _context.UserRoles
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (applicationUserRole == null)
            {
                return NotFound();
            }

            return View(applicationUserRole);
        }

        // GET: UserRole/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] ApplicationUserRole applicationUserRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUserRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUserRole);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UserId,RoleId")] ApplicationUserRole applicationUserRole)
        {
            if (id != applicationUserRole.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUserRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserRoleExists(applicationUserRole.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUserRole);
        }

        // GET: UserRole/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUserRole = await _context.UserRoles
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (applicationUserRole == null)
            {
                return NotFound();
            }

            return View(applicationUserRole);
        }

        // POST: UserRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var applicationUserRole = await _context.UserRoles.FindAsync(id);
            _context.UserRoles.Remove(applicationUserRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserRoleExists(long id)
        {
            return _context.UserRoles.Any(e => e.UserId == id);
        }
    }
}
