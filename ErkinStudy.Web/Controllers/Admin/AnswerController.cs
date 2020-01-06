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
    public class AnswerController : Controller
    {
        private readonly AppDbContext _context;

        public AnswerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Question
        [Authorize]
        public IActionResult Index(long? questionId)
        {
            ViewBag.QuestionId = questionId;
            return questionId.HasValue
                ? View(_context.Answers.Where(x => x.Question.Id == questionId))
                : View();
        }

        // GET: Question/Details/5
        [Authorize]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Answers.FindAsync(id.Value);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Question/Create
        [Authorize]
        public IActionResult Create(long questionId)
        {
            ViewBag.QuestionId = questionId;
            return View();
        }

        // POST: Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Content, IsCorrect")] Answer answer, long? questionId)
        {
            if (ModelState.IsValid)
            {
                var question = await _context.Questions.FindAsync(questionId);
                answer.Question = question;

                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { questionId });
            }
            return View(answer);
        }

        // GET: Question/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        // POST: Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(long quizId, [Bind("Content, IsCorrect")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                _context.Update(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { quizId });
            }
            return View(answer);
        }

        // GET: Question/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var answer = await _context.Answers.FindAsync(id);
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { answer.Question.Id });
        }

    }
}
