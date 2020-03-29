using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Lessons;
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
        
        public async Task<List<Folder>> GetFoldersByTeacherId(long teacherId, bool withParent = false)
        {
            return withParent
                ? await _context.Folders.Where(x => x.ParentId.HasValue).Include(x => x.Lessons)
                    .Where(x => x.TeacherId == teacherId).ToListAsync()
                : await _context.Folders.Where(x => !x.ParentId.HasValue).Include(x => x.Lessons)
                    .Where(x => x.TeacherId == teacherId).ToListAsync();
        }

        public async Task<List<UserFolder>> GetApprovedUsers(long folderId)
        {
            return await _context.UserFolders.Include(x => x.Folder).Include(x => x.User).Where(x => x.FolderId == folderId)
                .ToListAsync();
        }

        public void ApproveUser(long folderId, long userId)
        {
            try
            {
                if (_context.UserFolders.Any(x => x.FolderId == folderId && x.UserId == userId))
                    _logger.LogError($"Ошибка при потверждение пользователя {userId} для папки {folderId}, Ex: уже есть такой пользователь и папка");
                var userFolder = new UserFolder() {UserId = userId, FolderId = folderId};
                _context.UserFolders.Add(userFolder); 
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при потверждение пользователя {userId} для папки {folderId}, {e}");
            }
        }

        public void DeleteUser(long folderId, long userId)
        {
            try
            {
                var userFolder =
                    _context.UserFolders.FirstOrDefault(x => x.FolderId == folderId && x.UserId == userId);
                if (userFolder != null)
                {
                    _context.UserFolders.Remove(userFolder);
                    _context.SaveChanges();
                }
                else 
                    _logger.LogError($"Ошибка при удаление пользователя {userId} для папки {folderId}, Ex: уже есть такой пользователь и папка");
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка при удаление пользователя {userId} для папки {folderId}, {e}");
            }
        }

        public bool IsUserHasAccess(long folderId, long userId)
        {
            var folder = _context.Folders.Include(x => x.UserFolders).FirstOrDefault(x => x.Id == folderId);
            if (folder == null)
                return false;
            if (folder.UserFolders.Any(x => x.UserId == userId) || folder.TeacherId == userId)
                return true;

            return folder.ParentId.HasValue && IsUserHasAccess(folder.ParentId.Value, userId);
        }

        public long GetFolderPrice(long id)
        {
            return _context.Folders.FirstOrDefault(x => x.Id == id)?.Price ?? 0;
        }

    }

}
