using System;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    public class ColController : Controller
    {
	    private readonly AppDbContext _context;
        private readonly ILogger<ColController> _logger;
        private readonly EmailService _emailService;

	    public ColController(AppDbContext context, EmailService emailService, ILogger<ColController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task<IActionResult> SendCallRequest(long id, string type = "Сабак")
        {
            try
            {
                _logger.LogInformation($"Отправка уведомлений, данные: {User.Identity.Name}, {id}, {type}");
                await _emailService.SendEmailAsync("Әруақ Әруақ! Сабакка запрос.", $"Аты: {User.Identity.Name}, LessonId: {id}, Таңдауы: {type}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при отправке уведомлений, данные:  {User.Identity.Name}, {id}, {type}", e);
                TempData["ErrorMessage"] = $"{e.Message}, {e.StackTrace}";
            }
            return Redirect("/Home");
        }
        public IActionResult Detail(long id)
	    {
		    var lesson = _context.Lessons.Include(x => x.Contents).Include(x => x.Folder).FirstOrDefaultAsync(x => x.Id == id).Result;
            return View(lesson);
        }
    }
}