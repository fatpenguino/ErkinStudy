﻿using System.Linq;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;

namespace ErkinStudy.Infrastructure.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public bool IsUserApproved(string userId, long courseId)
        {
            var userCourse =
                _context.UserOnlineCourses.FirstOrDefault(x => x.UserId == long.Parse(userId) && x.OnlineCourseId == courseId);
            return userCourse != null;
        }

        public bool IsUserHasLesson(long userId, long lessonId)
        {
            return _context.UserLessons.Any(x => x.UserId == userId && x.LessonId == lessonId);
        }

        public string GetFullName(string username)
        {
            var user = _userManager.FindByNameAsync(username).Result;
            return $"{user.FirstName}";
        }
    }
}
