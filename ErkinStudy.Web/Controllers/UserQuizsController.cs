using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities.Quizzes;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;

namespace ErkinStudy.Web.Controllers
{
    public class UserQuizsController : Controller
    {
        private readonly AppDbContext _context;

        public UserQuizsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users by QuizId
        [Authorize]
        public async Task<IActionResult> Index(long quizId)
        {
            var users = await _context.Users.Include(x => x.UserQuizzes).ToListAsync();
            ViewBag.QuizId = quizId;

            return View(users);
        }

        [Authorize]
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

        [Authorize]
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
