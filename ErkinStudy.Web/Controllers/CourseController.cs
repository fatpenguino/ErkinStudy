using System;
using System.IO;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<CourseController> _logger;
        public CourseController(AppDbContext context, ILogger<CourseController> logger,
            IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _logger = logger;
            _appEnvironment = appEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(long id)
        {
            try
            {
                var onlineCourse = await _context.OnlineCourses.Include(x => x.OnlineCourseWeeks).ThenInclude(x => x.Homeworks)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (onlineCourse != null)
                {
                    return View(onlineCourse);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при подтягивание курса по Id - {id}, {e}");
            }
            return RedirectToAction("Index","Home");
        }

        public async Task<ActionResult> DownloadHomework(string path, long courseId)
        {
           
            if (!System.IO.File.Exists(_appEnvironment.WebRootPath + path))
            {
                _logger.LogError($"Не нашли файл {path}, для курса {courseId}");
                TempData["ErrorMessage"] = "Үй жұмысы табылмады.";
                return RedirectToAction("Index", new { id = courseId });
            }
            var fileBytes = await System.IO.File.ReadAllBytesAsync(_appEnvironment.WebRootPath + path);
            //хитрим чтобы лишнию дату тюда сюда не гонять, переделать надо 
            return File(fileBytes, "application/force-download", path.Replace("/Homeworks/",string.Empty));
        }
    }
}