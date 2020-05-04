using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    [Authorize(Roles = "Teacher,Moderator,Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _appEnvironment;

        public AdminController(UserManager<ApplicationUser> userManager, AppDbContext dbContext, ILogger<AdminController> logger, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
            _appEnvironment = appEnvironment;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users(int count = 50)
        {
            ViewData["UserCount"] = _dbContext.Users.Count();
            var users = count == -1 ? await _userManager.Users.OrderByDescending(x => x.Id).ToListAsync() : await _userManager.Users.OrderByDescending(x => x.Id).Take(count).ToListAsync();
            var model = new List<UserViewModel>();
            foreach (var user in users)
            {
                var item = new UserViewModel
                {
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.UserName
                };
                model.Add(item);
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _dbContext.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]      
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var applicationUser = await _dbContext.Users.FindAsync(id);
            await _userManager.DeleteAsync(applicationUser);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ListMediaFiles()
        {
            return View(Directory.EnumerateFiles(_appEnvironment.WebRootPath + "\\Media", "*", SearchOption.AllDirectories).ToList());
        }

        public IActionResult UploadMedia(IFormFile image)
        {
             if (image != null)
             {
                 try
                 {
                     var path = "/Media/" + image.FileName;
                     if (System.IO.File.Exists(_appEnvironment.WebRootPath + path))
                         return RedirectToAction("ListMediaFiles");
                     image.CopyTo(new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create));
                 }
                 catch (Exception e)
                 {
                     _logger.LogError($"Ошибка при загрузке медиа, {e}");
                 }
             }
             return RedirectToAction("ListMediaFiles");
        }
        public IActionResult DeleteMedia(string path)
        {
            if (path != null)
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Ошибка при удалений медиа, {e}");
                }
            }
            return RedirectToAction("ListMediaFiles");
        }
    }
}