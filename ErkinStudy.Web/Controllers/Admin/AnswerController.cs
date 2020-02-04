using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using ErkinStudy.Domain.Entities.Quizzes;

namespace ErkinStudy.Web.Controllers.Admin
{
    public class AnswerController : Controller
    {
        private readonly AppDbContext _context;

        public AnswerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Answer
        [Authorize]
        public IActionResult Index(long? questionId)
        {
            if (!questionId.HasValue)
                return View();

            var quiz = _context.Questions
                .Include(x => x.Quiz)
                .FirstOrDefault(x => x.Id == questionId)?
                .Quiz;
            ViewBag.QuestionId = questionId;
            ViewBag.QuizId = quiz?.Id;
            
            return View(_context.Answers.Where(x => x.Question.Id == questionId));
        }

        // GET: Answer/Create
        [Authorize]
        public IActionResult Create(long questionId)
        {
            ViewBag.QuestionId = questionId;
            return View();
        }

        // POST: Answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [Authorize]
        public async Task<IActionResult> Create(Answer answer)
        {
            if (ModelState.IsValid)
            {
                var question = await _context.Questions.FindAsync(answer.QuestionId);
                answer.Question = question;

                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { answer.QuestionId });
            }
            return View(answer);
        }

        // GET: Answer/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers.Include(x => x.Question).FirstOrDefaultAsync(x => x.Id == id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        // POST: Answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        [Authorize]
        public async Task<IActionResult> Edit(Answer answer)
        {
            if (ModelState.IsValid)
            {
                _context.Update(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { answer.QuestionId });
            }
            return View(answer);
        }

        // GET: Answer/Delete/5
        [Authorize]
        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = _context.Answers
                .FirstOrDefault(x => x.Id == id);
            if (answer == null)
            {
                return NotFound();
            }
            _context.Answers.Remove(answer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { questionId = answer.QuestionId });
        }

    }
}
