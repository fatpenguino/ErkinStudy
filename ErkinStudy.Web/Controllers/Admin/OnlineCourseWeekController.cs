using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class OnlineCourseWeekController : Controller
    {
        private readonly AppDbContext _context;

        public OnlineCourseWeekController(AppDbContext context)
        {
            _context = context;
        }

        // GET: OnlineCourseWeek
        [Authorize]
        public IActionResult Index(long? onlineCourseId)
        {
            ViewBag.OnlineCourseId = onlineCourseId;
            return onlineCourseId.HasValue ? View(_context.OnlineCourseWeeks.Include(l => l.OnlineCourse).Where(x => x.OnlineCourseId == onlineCourseId).AsQueryable())
                : View(_context.OnlineCourseWeeks.Include(x => x.OnlineCourse).AsQueryable());
        }

        // GET: OnlineCourseWeek/Details/5
        [Authorize]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineCourseWeek = await _context.OnlineCourseWeeks
                .Include(o => o.OnlineCourse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (onlineCourseWeek == null)
            {
                return NotFound();
            }

            return View(onlineCourseWeek);
        }

        // GET: OnlineCourseWeek/Create
        [Authorize]
        public IActionResult Create(long onlineCourseId)
        {
            ViewBag.OnlineCourseId = onlineCourseId;
            return View();
        }

        // POST: OnlineCourseWeek/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,OnlineCourseId,Name,Description,StartDate,Order,StreamUrl")] OnlineCourseWeek onlineCourseWeek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(onlineCourseWeek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { onlineCourseId = onlineCourseWeek.OnlineCourseId});
            }
            return View(onlineCourseWeek);
        }

        // GET: OnlineCourseWeek/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineCourseWeek = await _context.OnlineCourseWeeks.FindAsync(id);
            if (onlineCourseWeek == null)
            {
                return NotFound();
            }
            return View(onlineCourseWeek);
        }

        // POST: OnlineCourseWeek/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(long id, [Bind("Id,OnlineCourseId,Name,Description,StartDate,Order,StreamUrl")] OnlineCourseWeek onlineCourseWeek)
        {
            if (id != onlineCourseWeek.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(onlineCourseWeek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OnlineCourseWeekExists(onlineCourseWeek.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { onlineCourseId = onlineCourseWeek.OnlineCourseId });
            }
            return View(onlineCourseWeek);
        }

        // GET: OnlineCourseWeek/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineCourseWeek = await _context.OnlineCourseWeeks
                .Include(o => o.OnlineCourse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (onlineCourseWeek == null)
            {
                return NotFound();
            }

            return View(onlineCourseWeek);
        }

        // POST: OnlineCourseWeek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var onlineCourseWeek = await _context.OnlineCourseWeeks.FindAsync(id);
            _context.OnlineCourseWeeks.Remove(onlineCourseWeek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { onlineCourseId = onlineCourseWeek.OnlineCourseId });
        }

        private bool OnlineCourseWeekExists(long id)
        {
            return _context.OnlineCourseWeeks.Any(e => e.Id == id);
        }
    }
}
