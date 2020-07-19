﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ErkinStudy.Web.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ErkinStudy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly EmailService _emailService;
        private readonly FolderService _folderService;
        private readonly QuizService _quizService;
        private readonly LessonService _lessonService;
        private readonly CourseService _courseService;


        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext, EmailService emailService, CourseService courseService, LessonService lessonService, FolderService folderService, QuizService quizService)
        {
	        _logger = logger;
	        _dbContext = dbContext;
            _emailService = emailService;
            _courseService = courseService;
            _lessonService = lessonService;
            _folderService = folderService;
            _quizService = quizService;
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
        public async Task<IActionResult> OnlineCourseSchedule(long onlineCourseId)
        {
            var onlineCourse = await _dbContext.OnlineCourses.Include(x => x.OnlineCourseWeeks).ThenInclude(x => x.Homeworks)
                .FirstOrDefaultAsync(x => x.Id == onlineCourseId);
            return View(onlineCourse);
        }
        public async Task<IActionResult> Folder(long id)
        {
            var childs = await _folderService.GetChilds(id);
            var courses = await _courseService.GetByFolderId(id);
            var quizzes = await _quizService.GetByFolderId(id);
            var lessons = await _lessonService.GetByFolderId(id);
            if (childs.Count == 0 && courses.Count == 1 && quizzes.Count == 0 && lessons.Count == 0)
               return RedirectToAction("Index", "Course",new { id = courses.First().Id});
            if (childs.Count == 0 && courses.Count == 0 && quizzes.Count == 1 && lessons.Count == 0)
               return RedirectToAction("Quiz","TakeQuiz", new { id = quizzes.First().Id });
            if (childs.Count == 0 && courses.Count == 0 && quizzes.Count == 0 && lessons.Count == 1)
               return RedirectToAction("Detail","Col", new { id = lessons.First().Id });
            return View(await _dbContext.Folders.FirstOrDefaultAsync(x => x.Id == id && x.IsActive));
        }
        public async Task<IActionResult> Tests()
        {
            var tests = await _dbContext.Quizzes.Where(x => x.IsActive).OrderBy(x => x.Order).ThenBy(x => x.Title).ToListAsync();
            return View(tests);
        }
        public IActionResult Privacy() 
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(string name, string email, string subject, string message)
        {
            try
            {
                await _emailService.SendEmailAsync(subject, $"Пообщаться: Аты - {name}, Email - {email}, Message: {message}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка во время отправки из формы контактов, {e}");
            }
            return RedirectToAction("Index");
        }
        public IActionResult Offer()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError($"Ошибка {HttpContext.TraceIdentifier}");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Landing(long id)
        {
            try
            {
                var folder = await _dbContext.Folders.FirstOrDefaultAsync(x => x.Id == id);
                if (folder != null)
                {
                    var json = JsonConvert.DeserializeObject<LandingPageJson>(folder.LandingPage);
                    var model = new LandingViewModel
                    {
                        Name = folder.Name,
                        Description = folder.Description,
                        FolderId = folder.Id,
                        Price = folder.Price,
                        TeacherId = folder.Price,
                        Media = new LandingMedia() {Path = json.MediaPath, Type = (MediaType) json.MediaType},
                        Text = json.Text
                    };
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при получение Landing Page по folderId - {id}, {e}");
            }
           
            return RedirectToAction("Index");
        }
    }
}
