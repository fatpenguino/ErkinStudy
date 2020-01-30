using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class OnlineCourseController : Controller
    {
        private readonly AppDbContext _context;

        public OnlineCourseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: OnlineCourse
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.OnlineCourses.ToListAsync());
        }

        // GET: OnlineCourse/Details/5
        [Authorize]
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
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: OnlineCourse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,NumberOfWeeks,Price,StartDate,EndDate,IsActive")] OnlineCourse onlineCourse)
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
        [Authorize]
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
            return View(onlineCourse);
        }

        // POST: OnlineCourse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,NumberOfWeeks,Price,StartDate,EndDate,IsActive")] OnlineCourse onlineCourse)
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
        [Authorize]
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
        [ValidateAntiForgeryToken]
        [Authorize]
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
