﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    [Authorize(Roles = "Teacher,Moderator,Admin")]
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
        
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
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
                    UserName = user.UserName
                };
                model.Add(item);
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _dbContext.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]      
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var applicationUser = await _dbContext.Users.FindAsync(id);
            await _userManager.DeleteAsync(applicationUser);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveOnlineCourse(long userId, long onlineCourseId = 4)
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