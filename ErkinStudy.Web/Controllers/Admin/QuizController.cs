using System;
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
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Moderator,Admin,Teacher")]
	public class QuizController : Controller
    {
	    private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<QuizController> _logger;
        public QuizController(AppDbContext dbContext, IWebHostEnvironment appEnvironment, ILogger<QuizController> logger)
        {
            _dbContext = dbContext;
            _appEnvironment = appEnvironment;
            _logger = logger;
        }

        // GET: Quiz
        [Authorize(Roles = "Admin,Moderator,Teacher")]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Quizzes.OrderByDescending(x => x.IsActive).ToListAsync());
        }

        // GET: Quiz/Create
        public IActionResult Create(long? folderId)
        {
            if (folderId.HasValue)
                ViewData["FolderId"] = folderId;
            else
                ViewData["FolderList"] = new SelectList(_dbContext.Folders.Select(x => new { x.Id, Name = $"{x.Name}-{x.Description}" }), "Id", "Name");
            return View();
        }

        // POST: Quiz/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Title,Type,FolderId,IsActive,Price,Order,Description,Color")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                await _dbContext.Quizzes.AddAsync(quiz);
                await _dbContext.SaveChangesAsync();
                return quiz.FolderId.HasValue ? RedirectToAction("Manage", "Folder", new { id = quiz.FolderId }) : RedirectToAction(nameof(Index));
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
            ViewData["FolderList"] = new SelectList(_dbContext.Folders.Select(x => new { x.Id, Name = $"{x.Name}-{x.Description}" }), "Id", "Name", quiz.FolderId);
            return View(quiz);
        }

        // POST: Quiz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,Type,FolderId,IsActive,Price,Order,Description,Color")] Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Quizzes.Update(quiz);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Manage", "Folder", new { id = quiz.FolderId });
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
            try
            {
                questions.ForEach(DeleteQuestionWithoutSaveChange);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
            _dbContext.Quizzes.Remove(quiz);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Manage", "Folder", new { id = quiz.FolderId });
        }

        public async Task<IActionResult> Scores(long id)
        {
            var scores = await _dbContext.QuizScores.Include(x => x.Quiz).Include(x => x.User).Where(x => x.QuizId == id)
                .OrderByDescending(x => x.TakenTime).ToListAsync();
            return View(scores);
        }

        public void DeleteQuestionWithoutSaveChange(Question question)
        {
            if (question.ImagePath != null)
            {
                try
                {
                    //System.IO.File.Delete(_appEnvironment.WebRootPath + question.ImagePath);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Ошибка при удалений question - {question.Id}, {e}");
                }
            }

            var answers = question.Answers;
            _dbContext.Answers.RemoveRange(answers);
            _dbContext.Questions.Remove(question);
        }
        [HttpGet]
        public IActionResult Clone(long id)
        {
            var quiz = _dbContext.Quizzes.FirstOrDefault(x => x.Id == id);
            ViewData["FolderList"] = new SelectList(_dbContext.Folders.Select(x => new { x.Id, Name = $"{x.Name}-{x.Description}" }), "Id", "Name");
            return View(quiz);
        }
        [HttpPost]
        public IActionResult Clone(long quizId, string title, string description, int order, long? folderId)
        {
            try
            {
                var quiz = _dbContext.Quizzes.Include(x => x.Questions).ThenInclude(x => x.Answers).FirstOrDefault(x => x.Id == quizId);
                if (quiz == null)
                    return RedirectToAction("Index");
                var clonedQuiz = new Quiz { Title = title, Description = description, Order = order, FolderId = folderId };
                _dbContext.Quizzes.Add(clonedQuiz);
                _dbContext.SaveChanges();
                foreach (var question in quiz.Questions)
                {
                    var clonedQuestion = new Question
                    {
                        Content = question.Content,
                        ImagePath = question.ImagePath,
                        QuizId = clonedQuiz.Id
                    };
                    _dbContext.Questions.Add(clonedQuestion);
                    _dbContext.SaveChanges();
                    foreach (var answer in question.Answers)
                    {
                        var clonedAnswer = new Answer
                        {
                            Content = answer.Content,
                            IsCorrect = answer.IsCorrect,
                            Question = clonedQuestion
                        };
                        _dbContext.Answers.Add(clonedAnswer);
                        _dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка во время клонирование {quizId}, {title}, {e.Message} - {e.StackTrace}");
            }
            

            return folderId != null ? RedirectToAction("Manage", "Folder", new { id = folderId }) : RedirectToAction("Index");
        }
    }
}
