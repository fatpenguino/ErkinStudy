using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Entities.Quizzes;
using ErkinStudy.Web.Models;
using ErkinStudy.Infrastructure.Services;
using System.Collections.Generic;

namespace ErkinStudy.Web.Controllers
{
    [Authorize]
    public class TakeQuizController : Controller
    {
        private readonly ILogger<TakeQuizController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public TakeQuizController(ILogger<TakeQuizController> logger, AppDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
	        _logger = logger;
	        _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var quizzes = await _dbContext.Quizzes.ToListAsync();
            return View(quizzes);
        }

        [Authorize(Roles = "Moderator,Admin")]
        public async Task<IActionResult> Preview(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return NotFound();

                var quiz = await _dbContext.Quizzes
                    .Include(x => x.Questions)
                    .ThenInclude(q => q.Answers)
                    .FirstOrDefaultAsync(x => x.Id == id);

                return View(nameof(Quiz), quiz);
            }
            catch (Exception e)
            {
                //we need change it!!!
                _logger.LogError($"Произошла ошибка во время подтягивание Quiz по id={id}, у пользователя {User.Identity.Name}, {e}");
                RedirectToAction(nameof(Index));
            }

            return View(nameof(Index));
        }
        
        public async Task<IActionResult> Quiz(long? id)
        {
            try
            {
                if (!id.HasValue)
                    throw new NotImplementedException();

                var currentUser = await _userManager.GetUserAsync(User);
                var shortQuiz = await _dbContext.Quizzes.FindAsync(id);

                //if ((!userService.IsUserHasQuiz(currentUser.Id, shortQuiz.Id) 
                //    && shortQuiz.Price != 0) || !shortQuiz.IsActive)
                //    return RedirectToAction("Tests", "Home");

                var quiz = await _dbContext.Quizzes
                    .Include(x => x.Questions)
                    .ThenInclude(q => q.Answers)
                    .FirstOrDefaultAsync(x => x.Id == id);

                return View(quiz);
            }
            catch (Exception e)
            {
                //we need change it!!!
                _logger.LogError($"Произошла ошибка во время подтягивание Quiz по id {id}, у пользователя {User.Identity.Name}, {e}");
                RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public JsonResult Check([FromBody] QuizAnswerViewModel quizAnswer)
        {
            byte score = 0;
            var quizId = Convert.ToInt32(quizAnswer.QuizId);

            var questions = _dbContext.Questions.Where(x => x.QuizId == quizId).Include(x => x.Answers).ToList();
            List<long> checkedAnswers = quizAnswer.CheckedAnswers.Select(long.Parse).ToList(); 

            foreach (var question in questions)
            {
                var allAns = question.Answers.ToList();
                var correctAnsId = allAns.Where(x => x.IsCorrect).Select(x => x.Id);
                var allAnsId = allAns.Select(x => x.Id);
                if (correctAnsId.Except(checkedAnswers).Any() ||
                checkedAnswers.Intersect(allAnsId).Count() != correctAnsId.Count())
                {
                    continue;
                }
                score++;
            }

            try
            {
                var scoreDb = new QuizScore
                {
                    UserId = Convert.ToInt64(_userManager.GetUserId(User)),
                    QuizId = Convert.ToInt64(quizAnswer.QuizId),
                    TakenTime = DateTime.Now,
                    Point = score
                };

                _dbContext.QuizScores.Add(scoreDb);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError($"Произошла ошибка во время проверки Quiz у - {User.Identity.Name}, {e}");
            }
            return Json(score);
        }

        public async Task<IActionResult> Results(long? id)
        {
            if (!id.HasValue)
                throw new NotImplementedException();
                
            var scores = await _dbContext.QuizScores.Include(x => x.User).Where(x => x.QuizId == id && x.UserId != 1).OrderBy(x => x.TakenTime).ToListAsync();

            var uniqueScore = new Dictionary<long, QuizScore>();
            foreach(var score in scores)
            {
                if (!uniqueScore.ContainsKey(score.UserId))
                {
                    uniqueScore.Add(score.UserId, score);
                }
            }

            var uniqueScoreList = uniqueScore.Select(x => x.Value).OrderByDescending(x => x.Point).ToList();
            

            //Вот это было бы крутое решение
            //var scoresz = await _dbContext.QuizScores.Include(x => x.User).Where(x => x.QuizId == id).OrderBy(x => x.TakenTime).GroupBy(x => x.UserId).Select(x => x.First()).ToListAsync();


            return View(uniqueScoreList);

        }
    }
}