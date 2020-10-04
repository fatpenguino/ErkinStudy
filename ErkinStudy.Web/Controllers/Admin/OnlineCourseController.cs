using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin,Teacher")]
    public class OnlineCourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OnlineCourseController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: OnlineCourse
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!await _userManager.IsInRoleAsync(user, "Teacher"))
                return View(await _context.OnlineCourses.OrderByDescending(x => x.IsActive)
                    .ToListAsync());
            return View(_context.OnlineCourses);

        }

        // GET: OnlineCourse/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineCourse = await _context.OnlineCourses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (onlineCourse == null)
            {
                return NotFound();
            }

            return View(onlineCourse);
        }

        // GET: OnlineCourse/Create
        public IActionResult Create(long? folderId)
        {
            if (folderId.HasValue)
                ViewData["FolderId"] = folderId;
            else
                ViewData["FolderList"] = new SelectList(_context.Folders.Select(x => new { x.Id, Name = $"{x.Name}-{x.Description}" }), "Id", "Name");
            return View();
        }

        // POST: OnlineCourse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,FolderId,NumberOfWeeks,Price,Color,IsActive")] OnlineCourse onlineCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(onlineCourse);
                await _context.SaveChangesAsync();
                return onlineCourse.FolderId.HasValue ? RedirectToAction("Manage", "Folder", new { id = onlineCourse.FolderId }) : RedirectToAction(nameof(Index));
            }
            return View(onlineCourse);
        }

        // GET: OnlineCourse/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineCourse = await _context.OnlineCourses.FindAsync(id);
            if (onlineCourse == null)
            {
                return NotFound();
            }
            ViewData["FolderList"] = new SelectList(_context.Folders.Select(x => new { x.Id, Name = $"{x.Name}-{x.Description}" }), "Id", "Name", onlineCourse.FolderId);
            return View(onlineCourse);
        }

        // POST: OnlineCourse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,FolderId,NumberOfWeeks,Price,StartDate,EndDate,Color,IsActive")] OnlineCourse onlineCourse)
        {
            if (id != onlineCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(onlineCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OnlineCourseExists(onlineCourse.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction("Manage", "Folder", new { id = onlineCourse.FolderId });
            }
            return View(onlineCourse);
        }

        // GET: OnlineCourse/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineCourse = await _context.OnlineCourses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (onlineCourse == null)
            {
                return NotFound();
            }

            return View(onlineCourse);
        }

        // POST: OnlineCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var onlineCourse = await _context.OnlineCourses.FindAsync(id);
            _context.OnlineCourses.Remove(onlineCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction("Manage", "Folder", new { id = onlineCourse.FolderId });
        }

        private bool OnlineCourseExists(long id)
        {
            return _context.OnlineCourses.Any(e => e.Id == id);
        }
    }
}
