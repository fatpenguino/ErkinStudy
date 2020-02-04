using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ErkinStudy.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ErkinStudy.Domain.Entities.Identity;
using System.Collections.Generic;

namespace ErkinStudy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly EmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext, EmailService emailService,
            UserManager<ApplicationUser> userManager)
        {
	        _logger = logger;
	        _dbContext = dbContext;
            _emailService = emailService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var folders = await _dbContext.Folders.Where(x => x.IsActive).ToListAsync();
            return View(folders);
        }
        public async Task<IActionResult> SendCallRequest(string name, string number, string type = "Онлайн курс")
        {
            try
            {
                _logger.LogInformation($"Отправка уведомлений, данные: {name}, {number}, {type}");
                await _emailService.SendEmailAsync("Әруақ Әруақ! Жаңа адам.",$"Аты: {name}, Нөмірі: {number}, Таңдауы: {type}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при отправке уведомлений, данные: {name}, {number}, {type}", e);
                TempData["ErrorMessage"] = $"{e.Message}, {e.StackTrace}";
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> FreeCourse()
        {
            return View( await _dbContext.OnlineCourses.Include(x => x.OnlineCourseWeeks).FirstOrDefaultAsync(x => x.Id == 3));
        }
        public async Task<IActionResult> OnlineCourseSchedule(long onlineCourseId)
        {
            var onlineCourse = await _dbContext.OnlineCourses.Include(x => x.OnlineCourseWeeks).ThenInclude(x => x.Homeworks)
                .FirstOrDefaultAsync(x => x.Id == onlineCourseId);
            return View(onlineCourse);
        }
        public async Task<IActionResult> Folder(long id)
        {
            return View(await _dbContext.Folders.Include(x => x.Lessons).FirstOrDefaultAsync(x => x.Id == id && x.IsActive));
        }
        public async Task<IActionResult> Tests()
        {
            var tests = await _dbContext.Quizzes.Where(x => x.IsActive).ToListAsync();
            return View(tests);
        }
        public IActionResult Privacy() 
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError($"Ошибка {HttpContext.TraceIdentifier}");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
