using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ErkinStudy.Domain.Entities.Quizzes;
using ErkinStudy.Web.Models.Quiz;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Moderator,Admin,Teacher")]
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(AppDbContext context, IWebHostEnvironment appEnvironment, ILogger<QuestionController> logger)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _logger = logger;
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
                try
                {
                    string path = null;
                    if (questionViewModel.Image != null)
                    {
                        var uniqueFileName = Guid.NewGuid() + "_" + questionViewModel.Image.FileName;
                        path = "/Questions/" + uniqueFileName;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await questionViewModel.Image.CopyToAsync(fileStream);
                        }
                    }

                    var question = new Question()
                    {
                        QuizId = questionViewModel.QuizId,
                        Content = questionViewModel.Content,
                        ImagePath = path
                    };

                    questionViewModel.Answers.ForEach(x => x.Question = question);
                    question.Answers = questionViewModel.Answers;

                    _context.Add(question);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { question.QuizId });
                }
                catch (Exception e)
                {
                   _logger.LogError($"Ошибка при добавлений нового вопроса, {e}");
                }
            }
            return View(questionViewModel);
        }

        // GET: Question/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            ViewBag.ImageFileName = question.ImagePath;
            
            var questionViewModel =  new QuestionViewModel(){
                QuizId = question.QuizId,
                Content = question.Content,
                Answers = question.Answers.ToList()
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
                try
                {
                    var question = await _context.Questions.FindAsync(questionViewModel.Id);
                    var path = question.ImagePath;

                    if (questionViewModel.Image != null)
                    {
                        var uniqueFileName = Guid.NewGuid() + "_" + questionViewModel.Image.FileName;
                        path = "/Questions/" + uniqueFileName;
                        await questionViewModel.Image.CopyToAsync(new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create));

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

                    questionViewModel.Answers.ForEach(x => x.Question = question);
                    question.Answers = questionViewModel.Answers;

                    _context.Update(question);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { question.QuizId });

                }
                catch (Exception e)
                {
                    _logger.LogError($"Ошибка во время редактирование вопроса -{questionViewModel.Id}, {e}");
                }
            }
            return View(questionViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult AddAnswer([Bind("Answers,Id")] QuestionViewModel question)
        {
            question.Answers.Add(new Answer(){ QuestionId = question.Id });
            return PartialView("Answer", question);
        }

        // GET: Question/Delete/5
        public async Task<ActionResult> Delete(long? id)
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

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { quizId = question.QuizId });
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
            _context.Answers.RemoveRange(answers);
            _context.Questions.Remove(question);
        }

        public async Task<ActionResult> UserAnswers(long id)
        {
            var question = await _context.Questions.Include(x => x.Answers).FirstAsync(x => x.Id == id);
            var quizScores =  await _context.QuizScores.Include(x => x.User).Include(x => x.UserAnswers).Where(x => x.QuizId == question.QuizId).ToListAsync();
            if (quizScores.Count == 0)
                return RedirectToAction("Index", new {question.QuizId});

            var model = new UserAnswerViewModel
            {
                QuizId = question.QuizId, QuestionId = id, ImagePath = question.ImagePath, Content = question.Content, CorrectAnswer = question.Answers.FirstOrDefault()?.Content
            };
            foreach (var quizScore in quizScores)
            {
                var userAnswer = quizScore.UserAnswers.FirstOrDefault(x => x.QuestionId == id);
                if (userAnswer != null)
                {
                    model.FilledAnswers.Add(new FilledAnswer() { Answer = userAnswer.Answer, IsCorrect = userAnswer.IsCorrect, UserAnswerId = userAnswer.Id, UserEmail = quizScore.User.Email, UserFullName = $"{quizScore.User.LastName} {quizScore.User.FirstName}"});
                }
            }

            return View(model);
        }
        
        public async Task<int> MakeCorrect(long id)
        {
            try
            {
                var userAnswer = await _context.UserAnswers.Include(x => x.QuizScore).FirstAsync(x => x.Id == id);
                if (!userAnswer.IsCorrect)
                {
                    userAnswer.IsCorrect = true;
                    userAnswer.QuizScore.Point += 1;
                    _context.UserAnswers.Update(userAnswer);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при отмечаний ответа успешным id - {id}, {e}");
            }

            return 0;
        }

    }
}
