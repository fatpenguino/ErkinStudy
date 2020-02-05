using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities;
using ErkinStudy.Domain.Entities.Lessons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Moderator,Admin")]
    public class ContentController : Controller
    {
        private readonly AppDbContext _context;

        public ContentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Content
        public IActionResult Index(long? lessonId)
        {
	        ViewBag.LessonId = lessonId;
            return lessonId.HasValue ? View(_context.Contents.Include(x => x.Lesson).Where(x => x.LessonId == lessonId).AsQueryable()) 
									 : View(_context.Contents.Include(x => x.Lesson).AsQueryable());
        }

        // GET: Content/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // GET: Content/Create
        public IActionResult Create(long lessonId)
        {
	        ViewBag.LessonId = lessonId;
            return View();
        }

        // POST: Content/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("LessonId,Value,Order,ContentFormat,IsActive")] Content content)
        {
            if (ModelState.IsValid)
            {
                _context.Add(content);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { lessonId = content.LessonId });
            }
            return View(content);
        }

        // GET: Content/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }

        // POST: Content/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("Id,LessonId,Value,Order,ContentFormat,IsActive")] Content content)
        {
            if (id != content.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(content);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
	                if (!ContentExists(content.Id))
                    {
                        return NotFound();
                    }

	                throw;
                }
                return RedirectToAction(nameof(Index), new { lessonId = content.LessonId });
            }
            return View(content);
        }

        // GET: Content/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // POST: Content/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var content = await _context.Contents.FindAsync(id);
            var lessonId = content.LessonId;
            _context.Contents.Remove(content);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { lessonId });
        }

        private bool ContentExists(long id)
        {
            return _context.Contents.Any(e => e.Id == id);
        }
    }
}
