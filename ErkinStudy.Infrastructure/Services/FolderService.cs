using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities;
using ErkinStudy.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Infrastructure.Services
{
    public class FolderService
    {
        private readonly ILogger<FolderService> _logger;
        private readonly AppDbContext _context;

        public FolderService(AppDbContext context, ILogger<FolderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Folder>> GetAll()
        {
            return await _context.Folders.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<List<Folder>> GetChilds(long id)
        {
            return await _context.Folders.Where(x => x.IsActive && x.ParentId == id).ToListAsync();
        }
    }
}
