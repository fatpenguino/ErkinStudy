using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Infrastructure.Services;
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
        private readonly CourseService _courseService;
        public OnlineCourseController(AppDbContext context, UserManager<ApplicationUser> userManager, CourseService courseService)
        {
            _context = context;
            _userManager = userManager;
            _courseService = courseService;
        }

        // GET: OnlineCourse
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!await _userManager.IsInRoleAsync(user, "Teacher"))
                return View(await _context.OnlineCourses.Include(x => x.Category).OrderByDescending(x => x.IsActive)
                    .ToListAsync());
            var courses = await _courseService.GetCourseByUserId(user.Id);
            return View(courses);

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
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["FolderId"] = new SelectList(_context.Folders, "Id", "Id");
            return View();
        }

        // POST: OnlineCourse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,FolderId,CategoryId,NumberOfWeeks,Price,StartDate,EndDate,IsActive")] OnlineCourse onlineCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(onlineCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["FolderId"] = new SelectList(_context.Folders, "Id", "Id");
            return View(onlineCourse);
        }

        // POST: OnlineCourse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,FolderId,CategoryId,NumberOfWeeks,Price,StartDate,EndDate,IsActive")] OnlineCourse onlineCourse)
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
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        private bool OnlineCourseExists(long id)
        {
            return _context.OnlineCourses.Any(e => e.Id == id);
        }
    }
}
