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
            var userLesson =
                _context.UserLessons.FirstOrDefault(x => x.UserId == long.Parse(userId) && x.LessonId == 1);
            return userLesson != null;
        }
    }
}
