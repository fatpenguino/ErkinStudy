using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using ErkinStudy.Domain.Entities.Quizzes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Moderator,Admin")]
	public class QuizController : Controller
    {
	    private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _appEnvironment;
        public QuizController(AppDbContext dbContext, IWebHostEnvironment appEnvironment)
        {
            _dbContext = dbContext;
            _appEnvironment = appEnvironment;
        }

        // GET: Quiz
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Quizzes.Include(x => x.Questions).Include(x => x.Category).ToListAsync());
        }

        // GET: Quiz/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "Id", "Name");
            ViewData["FolderId"] = new SelectList(_dbContext.Folders, "Id", "Id");
            return View();
        }

        // POST: Quiz/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Title,CategoryId,FolderId,IsActive,Price,Order,Description")] Quiz quiz)
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
            ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "Id", "Name", quiz.CategoryId);
            ViewData["FolderId"] = new SelectList(_dbContext.Folders, "Id", "Id", quiz.FolderId);
            return View(quiz);
        }

        // POST: Quiz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,CategoryId,FolderId,IsActive,Price,Order,Description")] Quiz quiz)
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

        // POST: Quiz/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
	        var quiz = await _dbContext.Quizzes
                .Include(x => x.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);

            List<Question> questions = (List<Question>)quiz.Questions;
            QuestionController questionController = new QuestionController(_dbContext, _appEnvironment);

            try
            {
                questions.ForEach(x => questionController.DeleteQuestionWithoutSaveChange(x));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
            _dbContext.Quizzes.Remove(quiz);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Scores(long id)
        {
            var scores = await _dbContext.QuizScores.Include(x => x.User).Where(x => x.QuizId == id)
                .OrderByDescending(x => x.TakenTime).ToListAsync();
            return View(scores);
        }
    }
}
