using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<ApplicationUser> userManager, AppDbContext dbContext, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<UserViewModel>();
            foreach (var user in users)
            {
                var item = new UserViewModel
                {
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.UserName,
                    IsApprovedOnlineCourse = _dbContext.UserOnlineCourses.Any(x => x.UserId == user.Id && x.OnlineCourseId == 1)
                };
                model.Add(item);
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> ApproveOnlineCourse(long userId, long onlineCourseId = 1)
        {
            try
            {
                _logger.LogInformation($"Попытка потверждение онлайн курса - {onlineCourseId} для пользователя - {userId}");
                var userOnlineCourse = new UserOnlineCourse { UserId = userId, OnlineCourseId = onlineCourseId };
                await _dbContext.UserOnlineCourses.AddAsync(userOnlineCourse);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Онлайн курс - {onlineCourseId} для пользователя - {userId} успешно подтвержден.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при подтверждений онлайн курса - {onlineCourseId} для пользователя - {userId}", e);
            }
            return RedirectToAction(nameof(Users));
        }
    }
}