﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Enums;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ErkinStudy.Web.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly CourseService _courseService;
        private readonly QuizService _quizService;


        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext, EmailService emailService, FolderService folderService, CourseService courseService, QuizService quizService)
        {
	        _logger = logger;
	        _dbContext = dbContext;
            _emailService = emailService;
            _folderService = folderService;
            _courseService = courseService;
            _quizService = quizService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SendCallRequest(string name, string number, string message = "Онлайн курс")
        {
            try
            {
                _logger.LogInformation($"Отправка уведомлений, данные: {name}, {number}, {message}");
                await _emailService.SendEmailAsync("Әруақ Әруақ! Жаңа адам.",$"Аты: {name}, Нөмірі: {number}, {message}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при отправке уведомлений, данные: {name}, {number}, {message}", e);
                TempData["ErrorMessage"] = $"{e.Message}, {e.StackTrace}";
            }
            return RedirectToAction("Index","Landing");
        }
        //public async Task<IActionResult> Folder(long id)
        //{
        //    var folder = await _dbContext.Folders.FirstOrDefaultAsync(x => x.Id == id);
        //    if (folder == null)
        //    {
        //        _logger.LogError($"Ошибка при открытие папки, не существует такой папки {id}");
        //        return RedirectToAction("Index");
        //    }
        //    var childs = await _folderService.GetChilds(id);
        //    var courses = await _courseService.GetByFolderId(id);
        //    var quizzes = await _quizService.GetByFolderId(id);
        //    if (childs.Count == 0 && courses.Count == 1 && quizzes.Count == 0)
        //        return RedirectToAction("Index", "Course", new { id = courses.First().Id });
        //    if (childs.Count == 0 && courses.Count == 0 && quizzes.Count == 1)
        //        return quizzes.First().Type == QuizType.MultipleChoice
        //            ? RedirectToAction("Quiz", "TakeQuiz", new { id = quizzes.First().Id })
        //            : RedirectToAction("OpenQuiz", "TakeQuiz", new { id = quizzes.First().Id });
        //    return View(folder);
        //}

        public async Task<IActionResult> Folder(long id)
        {
            var folder = await _dbContext.Folders.FirstOrDefaultAsync(x => x.Id == id);
            if (folder == null)
            {
                _logger.LogError($"Ошибка при открытие папки, не существует такой папки {id}");
                return RedirectToAction("Index");
            }

            var model = new FolderViewModel {Id = folder.Id, Price = folder.Price, Title = folder.Name, IsQuizGroup = folder.IsQuizGroup};

            var childs = _folderService.GetChildsEnumerable(id).Select(x => new FolderItem() { Id = x.Id, Order = x.Order, Price = x.Price, Title = x.Name, Color = x.Color, Type = FolderItemType.Folder});
            var courses = _folderService.GetCoursesEnumerable(id).Select(x => new FolderItem() { Id = x.Id, Order = x.Order, Price = x.Price, Title = x.Name, Color = x.Color, Type = FolderItemType.Course });
            var quizzes = _folderService.GetQuizzesEnumerable(id).Select(x => new FolderItem() { Id = x.Id, Order = x.Order, Title = x.Title, Color =x.Color, Type  = FolderItemType.Quiz, QuizType = x.Type });
            model.Items = childs.Union(courses).Union(quizzes);
            return View(model);
        }
        public async Task<IActionResult> Tests()
        {
            var tests = await _dbContext.Quizzes.Where(x => x.IsActive).OrderBy(x => x.Order).ThenBy(x => x.Title).ToListAsync();
            return View(tests);
        }

        public async Task<IActionResult> TotalRating(long id)
        {
            var totalRating = await _folderService.TotalRating(id);
            if (totalRating == null)
                return RedirectToAction("Folder", new { id });
            var model = new TotalRatingViewModel
            {
                FolderId = totalRating.FolderId,
                QuizIds = totalRating.QuizIds.ToArray(),
                QuizTitles = totalRating.QuizTitles.ToArray(),
                FolderName =  totalRating.FolderName,
                UserScores = totalRating.UserScores.Select(x => new UserScoreViewModel()
                {
                    FullName = x.FullName, UserId = x.UserId, Scores = x.Scores, TotalPoint = x.TotalPoint
                }).ToList()
            };
            return View(model);
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

        [Authorize]
        public IActionResult FeedBack()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FeedBack(string subject, string message, string anonymous)
        {
            try
            {
                message = anonymous == "on"
                    ? $"Кері байланыс: Аноним, Message: {message}"
                    : $"Кері байланыс: Email - {User.Identity.Name}, Message: {message}";
                await _emailService.SendEmailAsync(subject, message, "report@erkinstudy.kz");
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
                    if (string.IsNullOrWhiteSpace(folder.LandingPage))
                    {
                        _logger.LogError($"Попытка открыть выключенный landing, {id}");
                        return RedirectToAction("Folder", new {id});
                    }
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
