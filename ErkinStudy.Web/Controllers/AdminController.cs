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

namespace ErkinStudy.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;

        public AdminController(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
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
                    IsApprovedOnlineCourse = _dbContext.UserOnlineCourses.Any(x => x.UserId == user.Id)
                };
                model.Add(item);
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> UserLessons(long userId)
        {
            var model = new UserLessonViewModel();
            var items = new List<SelectListItem>();
            //var user = await _userManager.FindByIdAsync(userId.ToString());
            var lessons = await _dbContext.Lessons.Include(x => x.Folder).ToListAsync();
            var userLessons = await _dbContext.UserLessons.Where( x => x.UserId == userId).ToListAsync();
            foreach (var lesson in lessons)
            {
                var item = new SelectListItem
                {
                    Text = $"{lesson.Folder.Name} - {lesson.Name}",
                    Value = lesson.Id.ToString(),
                    Selected = userLessons.Any(x => x.LessonId == lesson.Id)
                };
                items.Add(item);
            }
            model.SelectListItems = items;
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> UpdateUserLessons(UserLessonViewModel model)
        {
            foreach (var item in model.SelectListItems)
            {
                var userLesson = new UserLesson {UserId = model.UserId, LessonId = long.Parse(item.Value)};
                await _dbContext.UserLessons.AddAsync(userLesson);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> ApproveOnlineCourse(long userId)
        {
            var userOnlineCourse = new UserOnlineCourse {UserId = userId, OnlineCourseId = 1};
            await _dbContext.UserOnlineCourses.AddAsync(userOnlineCourse);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }
    }
}