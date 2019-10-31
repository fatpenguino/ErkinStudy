using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Infrastructure.Context;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class ParagraphController : Controller
    {
        private readonly AppDbContext _context;

        public ParagraphController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Paragraph
        public IActionResult Index(long? degreeId)
        {
	        ViewBag.DegreeId = degreeId;
	        return degreeId.HasValue
		        ? View(_context.Paragraphs.Include(x => x.Degree).Where(x => x.DegreeId == degreeId).AsQueryable())
		        : View(_context.Paragraphs.Include(x => x.Degree).AsQueryable());
        }

        // GET: Paragraph/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paragraph = await _context.Paragraphs.Include(x => x.Degree)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paragraph == null)
            {
                return NotFound();
            }

            return View(paragraph);
        }

        // GET: Paragraph/Create
        public IActionResult Create(long degreeId)
        {
	        ViewBag.DegreeId = degreeId;
            return View();
        }

        // POST: Paragraph/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Order,DegreeId,IsActive")] Paragraph paragraph)
        {
            if (ModelState.IsValid)
            {
				paragraph.CreatedAt = DateTime.Now;
                _context.Add(paragraph);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { degreeId = paragraph.DegreeId });
            }
            return View(paragraph);
        }

        // GET: Paragraph/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paragraph = await _context.Paragraphs.FindAsync(id);
            if (paragraph == null)
            {
                return NotFound();
            }
            return View(paragraph);
        }

        // POST: Paragraph/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,Order,DegreeId,IsActive")] Paragraph paragraph)
        {
            if (id != paragraph.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paragraph);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
	                if (!ParagraphExists(paragraph.Id))
                    {
                        return NotFound();
                    }

	                throw;
                }
                return RedirectToAction(nameof(Index), new { degreeId = paragraph.DegreeId });
            }
            return View(paragraph);
        }

        // GET: Paragraph/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paragraph = await _context.Paragraphs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paragraph == null)
            {
                return NotFound();
            }

            return View(paragraph);
        }

        // POST: Paragraph/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var paragraph = await _context.Paragraphs.FindAsync(id);
            _context.Paragraphs.Remove(paragraph);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { degreeId = paragraph.DegreeId });
        }

        private bool ParagraphExists(long id)
        {
            return _context.Paragraphs.Any(e => e.Id == id);
        }
    }
}
