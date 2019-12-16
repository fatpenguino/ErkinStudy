using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ErkinStudy.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;
        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
	        _logger = logger;
	        _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var subjects = await _dbContext.Subjects.Include(x => x.Degrees).Where(x => x.IsActive).ToListAsync();
            return View(subjects);
        }
        public async Task<IActionResult> Subjects()
        {
            var subjects = await _dbContext.Subjects.Include(x => x.Degrees).Where(x => x.IsActive).ToListAsync();
            return View(subjects);
        }

        public async Task<IActionResult> OnlineCourseSchedule(long onlineCourseId)
        {
            var onlineCourse = await _dbContext.OnlineCourses.Include(x => x.OnlineCourseWeeks)
                .FirstOrDefaultAsync(x => x.Id == onlineCourseId);
            return View(onlineCourse);
        }
        public IActionResult Privacy() 
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
