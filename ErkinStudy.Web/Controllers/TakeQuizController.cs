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

namespace ErkinStudy.Web.Controllers
{
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
        
        [Authorize]
        public async Task<IActionResult> Quiz(long? id)
        {
            try
            {
                if (!id.HasValue)
                    throw new NotImplementedException();

                var currentUser = await _userManager.GetUserAsync(User);
                var userService = new UserService(_dbContext, _userManager);
                var shortQuiz = await _dbContext.Quizzes.FindAsync(id);

                if ((!userService.IsUserHasQuiz(currentUser.Id, shortQuiz.Id) 
                    && shortQuiz.Price != 0) || !shortQuiz.IsActive)
                    return RedirectToAction("Tests", "Home");

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
        [Authorize]
        public JsonResult Check([FromBody] QuizAnswerViewModel quizAnswer)
        {
            int score = 0;
            try
            {
                foreach (var ansId in quizAnswer.CheckedAnswers)
                {
                    var answer = _dbContext.Answers.Find(Convert.ToInt64(ansId));
                    if (answer != null && answer.IsCorrect)
                        score++;
                }

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
    }
}