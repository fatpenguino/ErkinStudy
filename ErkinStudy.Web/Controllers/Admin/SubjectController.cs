using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ErkinStudy.Domain.Models;
using ErkinStudy.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class SubjectController : Controller
    {
	    private readonly AppDbContext _dbContext;
        public SubjectController(AppDbContext dbContext)
        {
	        _dbContext = dbContext;
        }

        // GET: Subject
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Subjects.ToListAsync());
        }

        // GET: Subject/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _dbContext.Subjects.FindAsync(id.Value);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subject/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,State")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                await _dbContext.Subjects.AddAsync(subject);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subject/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subject = await _dbContext.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subject/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,State")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Subjects.Update(subject);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subject/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _dbContext.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Degree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
	        var subject = await _dbContext.Subjects.FindAsync(id);
           _dbContext.Subjects.Remove(subject);
           await _dbContext.SaveChangesAsync();
           return RedirectToAction(nameof(Index));
        }
    }
}
