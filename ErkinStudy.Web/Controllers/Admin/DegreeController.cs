using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Models;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class DegreeController : Controller
    {
	    private readonly AppDbContext _context;
        public DegreeController(AppDbContext context)
        {
	        _context = context;
        }

        // GET: Degree
        public  IActionResult Index(long? subjectId)
        {
	        ViewBag.SubjectId = subjectId;
	        return subjectId.HasValue
		        ? View( _context.Degrees.Include(x => x.Subject).Where(x => x.SubjectId == subjectId).AsQueryable())
		        : View( _context.Degrees.Include(x => x.Subject).AsQueryable());
        }

        // GET: Degree/Details/5
 
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degree = await _context.Degrees.Include(x => x.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (degree == null)
            {
                return NotFound();
            }

            return View(degree);
        }

        // GET: Degree/Create
        public IActionResult Create(long? subjectId)
        {
	        ViewBag.SubjectId = subjectId;
            return View();
        }

        // POST: Degree/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Level,Name,Description,SubjectId")] Degree degree)
        {
            if (ModelState.IsValid)
            {
                _context.Add(degree);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {subjectId = degree.SubjectId});
            }
            return View(degree);
        }

        // GET: Degree/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degree = await _context.Degrees.FindAsync(id);
            if (degree == null)
            {
                return NotFound();
            }
            return View(degree);
        }

        // POST: Degree/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Level,Name,Description,SubjectId")] Degree degree)
        {
            if (id != degree.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(degree);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
	                if (!DegreeExists(degree.Id))
                    {
                        return NotFound();
                    }

	                throw;
                }
                return RedirectToAction(nameof(Index), new { subjectId = degree.SubjectId});
            }
            return View(degree);
        }

        // GET: Degree/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degree = await _context.Degrees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (degree == null)
            {
                return NotFound();
            }

            return View(degree);
        }

        // POST: Degree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var degree = await _context.Degrees.FindAsync(id);
            var subjectId = degree.SubjectId;
            _context.Degrees.Remove(degree);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { subjectId = subjectId });
        }

        private bool DegreeExists(long id)
        {
            return _context.Degrees.Any(e => e.Id == id);
        }
    }
}
