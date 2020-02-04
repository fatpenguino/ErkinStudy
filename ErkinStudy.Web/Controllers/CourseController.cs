using System;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CourseController> _logger;
        public CourseController(AppDbContext context, ILogger<CourseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(long id)
        {
            try
            {
                var onlineCourse = await _context.OnlineCourses.Include(x => x.OnlineCourseWeeks).ThenInclude(x => x.Homeworks)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return View(onlineCourse);
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при подтягивание курса по Id - {id}, {e}");
            }
            return View();
        }
    }
}