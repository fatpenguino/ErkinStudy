using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Lessons;
using ErkinStudy.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Infrastructure.Services
{
     public class LessonService
     {
         private readonly AppDbContext _context;
         private readonly ILogger<LessonService> _logger;

         public LessonService(AppDbContext context, ILogger<LessonService> logger)
         {
             _context = context;
             _logger = logger;
         }

         public async Task<List<Lesson>> GetByFolderId(long folderId, bool active = true)
         {
             return active ? await _context.Lessons.Where(x => x.FolderId == folderId && x.IsActive).ToListAsync() : await _context.Lessons.Where(x => x.FolderId == folderId).ToListAsync();
         }

         public async Task<Lesson> GetById(long lessonId)
         {
             return  await _context.Lessons.FindAsync(lessonId);
         }
     }
}
