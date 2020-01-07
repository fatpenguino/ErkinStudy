using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ErkinStudy.Web.Models;
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
    }
}