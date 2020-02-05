using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserOnlineCourseController : Controller
    {
        private readonly AppDbContext _context;

        public UserOnlineCourseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserOnlineCourse
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserOnlineCourses.Include(u => u.OnlineCourse).Include(u => u.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserOnlineCourse/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOnlineCourse = await _context.UserOnlineCourses
                .Include(u => u.OnlineCourse)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.OnlineCourseId == id);
            if (userOnlineCourse == null)
            {
                return NotFound();
            }

            return View(userOnlineCourse);
        }

        // GET: UserOnlineCourse/Create
        public IActionResult Create()
        {
            ViewData["OnlineCourseId"] = new SelectList(_context.OnlineCourses, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: UserOnlineCourse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("UserId,OnlineCourseId,IsActive")] UserOnlineCourse userOnlineCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userOnlineCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OnlineCourseId"] = new SelectList(_context.OnlineCourses, "Id", "Id", userOnlineCourse.OnlineCourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOnlineCourse.UserId);
            return View(userOnlineCourse);
        }

        // GET: UserOnlineCourse/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOnlineCourse = await _context.UserOnlineCourses.FindAsync(id);
            if (userOnlineCourse == null)
            {
                return NotFound();
            }
            ViewData["OnlineCourseId"] = new SelectList(_context.OnlineCourses, "Id", "Id", userOnlineCourse.OnlineCourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOnlineCourse.UserId);
            return View(userOnlineCourse);
        }

        // POST: UserOnlineCourse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("UserId,OnlineCourseId,IsActive")] UserOnlineCourse userOnlineCourse)
        {
            if (id != userOnlineCourse.OnlineCourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userOnlineCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserOnlineCourseExists(userOnlineCourse.OnlineCourseId))
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
            ViewData["OnlineCourseId"] = new SelectList(_context.OnlineCourses, "Id", "Id", userOnlineCourse.OnlineCourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOnlineCourse.UserId);
            return View(userOnlineCourse);
        }
        
        public async Task<IActionResult> Delete(long userId, long courseId)
        {
            var userOnlineCourse = await _context.UserOnlineCourses
                .Include(u => u.OnlineCourse)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.OnlineCourseId == courseId && m.UserId == userId);
            if (userOnlineCourse == null)
            {
                return NotFound();
            }

            return View(userOnlineCourse);
        }

        // POST: UserOnlineCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long userId, long onlineCourseId)
        {
            var userOnlineCourse = await _context.UserOnlineCourses.FirstOrDefaultAsync(x => x.UserId == userId && x.OnlineCourseId == onlineCourseId);
            _context.UserOnlineCourses.Remove(userOnlineCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserOnlineCourseExists(long id)
        {
            return _context.UserOnlineCourses.Any(e => e.OnlineCourseId == id);
        }
    }
}
