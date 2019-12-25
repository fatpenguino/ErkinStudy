using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Infrastructure.Context;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class LessonController : Controller
    {
        private readonly AppDbContext _context;

        public LessonController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Lesson
        public IActionResult Index(long? paragraphId)
        {
	        ViewBag.ParagraphId = paragraphId;
            return paragraphId.HasValue ? View(_context.Lessons.Include(l => l.Folder).Where(x => x.FolderId == paragraphId).AsQueryable()) 
										: View(_context.Lessons.Include(x => x.Folder).AsQueryable());
        }

        // GET: Lesson/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .Include(l => l.Folder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // GET: Lesson/Create
        public IActionResult Create(long paragraphId)
        {
	        ViewBag.ParagraphId = paragraphId;
            return View();
        }

        // POST: Lesson/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FolderId,Name,Description,Order,Price,IsActive")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
				lesson.CreatedAt = DateTime.Now;
				_context.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { paragraphId = lesson.FolderId });
            }
            return View(lesson);
        }

        // GET: Lesson/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }

        // POST: Lesson/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FolderId,Name,Description,Order,Price,IsActive")] Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
	                if (!LessonExists(lesson.Id))
                    {
                        return NotFound();
                    }

	                throw;
                }
                return RedirectToAction(nameof(Index), new { paragraphId = lesson.FolderId });
            }
            return View(lesson);
        }

        // GET: Lesson/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .Include(l => l.Folder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lesson/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            var paragraphId = lesson.FolderId;
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { paragraphId });
        }

        private bool LessonExists(long id)
        {
            return _context.Lessons.Any(e => e.Id == id);
        }
    }
}
