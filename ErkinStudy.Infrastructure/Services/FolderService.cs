﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Lessons;
using ErkinStudy.Domain.Entities.OnlineCourses;
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

        public string GetFolderName(long id)
        {
            if (id == -1)
                return string.Empty;
            var folder = _context.Folders.Find(id);
            return folder != null ? folder.Name : string.Empty;
        }

        public async Task<List<Folder>> GetChilds(long id, bool active = true)
        {
            return active ? await _context.Folders.Where(x => x.IsActive && x.ParentId == id).ToListAsync() : await _context.Folders.Where(x => x.ParentId == id).ToListAsync();
        }
        
        public async Task<List<Folder>> GetFoldersByTeacherId(long teacherId)
        {
            return await _context.Folders.Where(x => x.TeacherId == teacherId).ToListAsync();
        }
    }
}
