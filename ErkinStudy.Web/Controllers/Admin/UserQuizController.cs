using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities.Quizzes;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;

namespace ErkinStudy.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserQuizController : Controller
    {
        private readonly AppDbContext _context;

        public UserQuizController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users by QuizId
        public async Task<IActionResult> Index(long quizId)
        {
            var users = await _context.Users.Include(x => x.UserQuizzes).ToListAsync();
            ViewBag.QuizId = quizId;

            return View(users);
        }

        public async Task<IActionResult> Approve(long quizId, long userId)
        {
            var userQuiz = new UserQuiz()
            {
                QuizId = quizId,
                UserId = userId
            };

            _context.Add(userQuiz);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { quizId });
        }

        public async Task<IActionResult> Remove(long quizId, long userId)
        {
            var userQuiz = new UserQuiz()
            {
                QuizId = quizId,
                UserId = userId
            };

            _context.Remove(userQuiz);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { quizId });
        }

    }
}
