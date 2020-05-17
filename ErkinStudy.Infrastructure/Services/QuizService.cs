using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Quizzes;
using ErkinStudy.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Infrastructure.Services
{
    public class QuizService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<QuizService> _logger;

        public QuizService(AppDbContext context, ILogger<QuizService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Quiz>> GetByFolderId(long folderId, bool active = true)
        {
            try
            {
                var quizzes = active ? await _context.Quizzes.Where(x =>  x.FolderId == folderId && x.IsActive)
                    .OrderBy(x => x.Order).ThenBy(x => x.Title).ToListAsync() : await _context.Quizzes.Where(x => x.FolderId == folderId)
                    .OrderBy(x => x.Order).ThenBy(x => x.IsActive ? 0 : 1).ThenBy(x => x.Title).ToListAsync();
                return quizzes;
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при подтягиваний тестов по folderId - {folderId}, {e}");
            }
            return new List<Quiz>();
        }

        public async Task<Quiz> GetById(long id)
        {
            return await _context.Quizzes.FindAsync(id);
        }
    }
}
