using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using ErkinStudy.Web.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ErkinStudy.Domain.Entities.Quizzes;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Moderator,Admin,Teacher")]
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public QuestionController(AppDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Question
        public IActionResult Index(long? quizId)
        {
            ViewBag.QuizId = quizId;
            return quizId.HasValue
                ? View(_context.Questions.Include(x => x.Quiz).Include(x => x.Answers).Where(x => x.Quiz.Id == quizId))
                : View();
        }

        // GET: Question/Create
        public IActionResult Create(long quizId)
        {
            ViewBag.QuizId = quizId;
            return View();
        }

        // POST: Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(QuestionViewModel questionViewModel)
        {
            if (ModelState.IsValid)
            {
                string path = null;
                if (questionViewModel.Image != null)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + questionViewModel.Image.FileName;
                    path = "/Questions/" + uniqueFileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await questionViewModel.Image.CopyToAsync(fileStream);
                    }
                }
                
                var question = new Question(){
                    QuizId = questionViewModel.QuizId,
                    Content = questionViewModel.Content,
                    ImagePath = path
                };
                
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { question.QuizId });
            }
            return View(new ErrorViewModel());
        }

        // GET: Question/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(x => x.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            ViewBag.ImageFileName = question.ImagePath;
            
            var questionViewModel =  new QuestionViewModel(){
                QuizId = question.QuizId,
                Content = question.Content
            };
            return View(questionViewModel);
        }

        // POST: Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(QuestionViewModel questionViewModel)
        {
            if (ModelState.IsValid)
            {
                var question = _context.Questions.Find(questionViewModel.Id);
                string path = question.ImagePath;

                if (questionViewModel.Image != null)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + questionViewModel.Image.FileName;
                    path = "/Questions/" + uniqueFileName;
                    questionViewModel.Image.CopyTo(new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create));
                    
                    if (question.ImagePath != null)
                    {
                        try
                        {
                            System.IO.File.Delete(_appEnvironment.WebRootPath + question.ImagePath);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            return RedirectToAction("Error", "Home");
                        }
                    }
                }
                
                question.Content = questionViewModel.Content;
                question.ImagePath = path;
                
                _context.Update(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { question.QuizId });
            }
            return View(new ErrorViewModel());
        }

        // GET: Question/Delete/5
        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = _context.Questions
                .Include(x => x.Answers)
                .FirstOrDefault(x => x.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            try
            {
                DeleteQuestionWithoutSaveChange(question);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { quizId = question.QuizId });
        }

        public void DeleteQuestionWithoutSaveChange(Question question)
        {
            if (question.ImagePath != null)
            {
                try
                {   
                    System.IO.File.Delete(_appEnvironment.WebRootPath + question.ImagePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw e;
                }
            }

            var answers = question.Answers;
            _context.Answers.RemoveRange(answers);
            _context.Questions.Remove(question);
        }

    }
}
