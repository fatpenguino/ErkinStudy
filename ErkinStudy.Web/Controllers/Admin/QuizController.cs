using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Quiz;
using Microsoft.AspNetCore.Mvc;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Web.Controllers.Admin
{
	public class QuizController : Controller
    {
	    private readonly AppDbContext _dbContext;
        public QuizController(AppDbContext dbContext)
        {
	        _dbContext = dbContext;
        }

        // GET: Quiz
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Quizzes.ToListAsync());
        }

        // GET: Quiz/Details/5
        [Authorize]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _dbContext.Quizzes.FindAsync(id.Value);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // GET: Quiz/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                await _dbContext.Quizzes.AddAsync(quiz);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quiz);
        }

        // GET: Quiz/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var quiz = await _dbContext.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }
            return View(quiz);
        }

        // POST: Quiz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title")] Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Quizzes.Update(quiz);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quiz);
        }

        // GET: Quiz/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _dbContext.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Folder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
	        var quiz = await _dbContext.Quizzes.FindAsync(id);
           _dbContext.Quizzes.Remove(quiz);
           await _dbContext.SaveChangesAsync();
           return RedirectToAction(nameof(Index));
        }
    }
}
