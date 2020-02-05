using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities;
using ErkinStudy.Domain.Entities.Lessons;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Moderator,Admin")]
    public class UserLessonController : Controller
    {
        private readonly AppDbContext _context;

        public UserLessonController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserLessons
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserLessons.Include(u => u.Lesson).Include(u => u.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserLessons/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLesson = await _context.UserLessons
                .Include(u => u.Lesson)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.LessonId == id);
            if (userLesson == null)
            {
                return NotFound();
            }

            return View(userLesson);
        }

        // GET: UserLessons/Create
        public IActionResult Create()
        {
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserLessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("UserId,LessonId,IsActive")] UserLesson userLesson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userLesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id", userLesson.LessonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userLesson.UserId);
            return View(userLesson);
        }

        // GET: UserLessons/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLesson = await _context.UserLessons.FindAsync(id);
            if (userLesson == null)
            {
                return NotFound();
            }
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id", userLesson.LessonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userLesson.UserId);
            return View(userLesson);
        }

        // POST: UserLessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("UserId,LessonId,IsActive")] UserLesson userLesson)
        {
            if (id != userLesson.LessonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userLesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserLessonExists(userLesson.LessonId))
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
            ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id", userLesson.LessonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userLesson.UserId);
            return View(userLesson);
        }

        // GET: UserLessons/Delete/5
        public async Task<IActionResult> Delete(long userId, long lessonId)
        {
            var userLesson = await _context.UserLessons
                .FirstOrDefaultAsync(m => m.LessonId == lessonId && m.UserId == userId);
            if (userLesson == null)
            {
                return NotFound();
            }

            return View(userLesson);
        }

        // POST: UserLessons/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long userId, long lessonId)
        {
            var userLesson = await _context.UserLessons
                .FirstOrDefaultAsync(m => m.LessonId == lessonId && m.UserId == userId);
            _context.UserLessons.Remove(userLesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserLessonExists(long id)
        {
            return _context.UserLessons.Any(e => e.LessonId == id);
        }
    }
}
