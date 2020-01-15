using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
public JsonResult AjaxTest2([FromBody] string testStr)
{
 
    return Json("Сервер получил данные: " + testStr);
 
}



        [HttpPost]
        public JsonResult Check(string quiz)
        {
            int score = 0;
            /*string[] questionIds = iFormCollection["questionId"];
            foreach(var qId in questionIds)
            {
                var question = await _dbContext.Questions
                    .Include(x => x.Answers)
                    .FirstOrDefaultAsync(x => x.Id == Convert.ToInt64(qId));
                if (question.Answers.First(x => x.IsCorrect).Id == Convert.ToInt64(iFormCollection[question.Id.ToString()]))
                {
                    score++;
                }
            }
*/
            var scoreDB = new QuizScore
            {
                UserId = Convert.ToInt64(_userManager.GetUserId(User)),
                QuizId = Convert.ToInt64(quiz),
                TakenTime = DateTime.Now,
                Point = score
            };

            _dbContext.QuizScores.Add(scoreDB);
            _dbContext.SaveChanges();

            return Json("9");
        }
    }
}