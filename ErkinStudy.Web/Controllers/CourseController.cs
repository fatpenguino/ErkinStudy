using System;
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
                return View(onlineCourse);
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при подтягивание курса по Id - {id}, {e}");
            }
            return View();
        }

        public async Task<ActionResult> DownloadHomework(long id)
        {
            var homework = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + homework.Path);
            return File(fileBytes, "application/force-download", homework.Name);
        }
    }
}