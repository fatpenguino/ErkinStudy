using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using ErkinStudy.Domain.Entities.Quiz;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Question
        [Authorize]
        public IActionResult Index(long? quizId)
        {
            ViewBag.QuizId = quizId;
            return quizId.HasValue
                ? View(_context.Questions.Where(x => x.Quiz.Id == quizId))
                : View();
        }

        // GET: Question/Create
        [Authorize]
        public IActionResult Create(long quizId)
        {
            ViewBag.QuizId = quizId;
            return View();
        }

        // POST: Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Content")] Question question, long? quizId)
        {
            if (ModelState.IsValid)
            {
                var quiz = await _context.Quizzes.FindAsync(quizId);
                question.Quiz = quiz;

                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { quizId });
            }
            return View(question);
        }

        // GET: Question/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                                .Include(x => x.Quiz)
                                .FirstOrDefaultAsync(x => x.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // POST: Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(long quizId, [Bind("Id,Content")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Update(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { quizId });
            }
            return View(question);
        }

        // GET: Question/Delete/5
        [Authorize]
        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = _context.Questions
                .Include(x => x.Quiz)
                .Include(x => x.Answers)
                .FirstOrDefault(x => x.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            var answers = question.Answers;

            _context.Answers.RemoveRange(answers);
            _context.Questions.Remove(question);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { quizId = question.Quiz.Id });
        }
    }
}
