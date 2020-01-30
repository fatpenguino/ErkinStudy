using System.Linq;
using ErkinStudy.Infrastructure.Context;

namespace ErkinStudy.Infrastructure.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public bool IsUserApproved(string userId)
        {
            var userCourse =
                _context.UserOnlineCourses.FirstOrDefault(x => x.UserId == long.Parse(userId) && x.OnlineCourseId == 1);
            return userCourse != null;
        }

        public bool IsUserHasLesson(long userId, long lessonId)
        {
            return _context.UserLessons.Any(x => x.UserId == userId && x.LessonId == lessonId);
        }
    }
}
