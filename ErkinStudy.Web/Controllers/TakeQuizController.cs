using System;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities.Quiz;
using Microsoft.AspNetCore.Identity;
using ErkinStudy.Domain.Entities.Identity;

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
        [Authorize]
        public async Task<IActionResult> Quiz(long? id)
        {
            if (!id.HasValue)
                throw new NotImplementedException();
                
            var quiz = await _dbContext.Quizzes
                .Include(x => x.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            return View(quiz);
        }

        public class QuizAnswer
        {
            public string quizId { get; set; }
            public string[] checkedAnswers { get; set; }
        }

        [HttpPost]
        public JsonResult Check([FromBody] QuizAnswer quizAnswer)
        {
            int score = 0;

            foreach(var ansId in quizAnswer.checkedAnswers)
            {
                var answer = _dbContext.Answers.Find(Convert.ToInt64(ansId));
                if (answer != null && answer.IsCorrect)
                    score++;
            }

            var scoreDB = new QuizScore
            {
                UserId = Convert.ToInt64(_userManager.GetUserId(User)),
                QuizId = Convert.ToInt64(quizAnswer.quizId),
                TakenTime = DateTime.Now,
                Point = score
            };

            _dbContext.QuizScores.Add(scoreDB);
            _dbContext.SaveChanges();

            return Json(score);
        }
    }
}