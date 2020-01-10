using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Web.Controllers
{
    public class TakeQuizController : Controller
    {
        private readonly ILogger<TakeQuizController> _logger;
        private readonly AppDbContext _dbContext;
        public TakeQuizController(ILogger<TakeQuizController> logger, AppDbContext dbContext)
        {
	        _logger = logger;
	        _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var quizzes = await _dbContext.Quizzes.ToListAsync();
            return View(quizzes);
        }

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

        public async Task<IActionResult> Check(IFormCollection iFormCollection)
        {
            int score = 0;
            string[] questionIds = iFormCollection["questionId"];
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

            return View("Result", score);
        }
    }
}