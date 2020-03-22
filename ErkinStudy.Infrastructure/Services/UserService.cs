using System.Collections.Generic;
using System.Linq;
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
            var lesson = _context.Lessons.FirstOrDefault(x => x.Id == lessonId);
            return lesson?.Price == 0 || _context.UserLessons.Any(x => x.UserId == userId && x.LessonId == lessonId);
        }

        public bool IsUserHasQuiz(long userId, long quizId)
        {
            return _context.UserQuizzes.Any(x => x.UserId == userId && x.QuizId == quizId);
        }

        public string GetFullNameByUserName(string username)
        {
            var user = _userManager.FindByNameAsync(username).Result;
            return $"{user?.FirstName}";
        }
        public string GetFullNameByUserId(long id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            return $"{user?.FirstName}";
        }
        public List<ApplicationUser> GetAllTeachers()
        {
            var teacherRole = _context.Roles.First(x => x.Name == "Teacher");
            var userRoles = _context.UserRoles.Where(x => x.RoleId == teacherRole.Id);
            return _context.Users.Where(x => userRoles.Any(r => r.UserId == x.Id)).ToList();
        }
    }
}
