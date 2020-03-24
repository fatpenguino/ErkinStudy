using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace ErkinStudy.Infrastructure.Services
{
    public class CourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context, ILogger<CourseService> logger)
        {
            _context = context;
        }

        public async Task<List<OnlineCourse>> GetByFolderId(long folderId, bool active = true)
        {
            return active ? await _context.OnlineCourses.Where(x => x.FolderId == folderId && x.IsActive).ToListAsync() : await _context.OnlineCourses.Where(x => x.FolderId == folderId).ToListAsync();
        }

        public async Task<List<OnlineCourse>> GetCourseByUserId(long userId)
        {
            return await _context.UserOnlineCourses.Include(x => x.OnlineCourse).Where(x => x.UserId == userId).Select(x => x.OnlineCourse).ToListAsync();
        }

        public async Task<OnlineCourse> GetById(long id)
        {
            return await _context.OnlineCourses.FindAsync(id);
        }
    }
}
