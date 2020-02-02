using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Infrastructure.Services
{
    public class CourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context, ILogger<CourseService> logger)
        {
            _context = context;
        }

        public async Task<List<OnlineCourse>> GetByFolderId(long folderId)
        {
            var courses = await _context.OnlineCourses.Where(x => x.FolderId == folderId).ToListAsync();
            return courses;
        }
    }
}
