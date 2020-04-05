using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Entities.Lessons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin, Teacher")]
    public class FolderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly FolderService _folderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FolderController(AppDbContext context, UserService userService, FolderService folderService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userService = userService;
            _folderService = folderService;
            _userManager = userManager;
        }

        // GET: Folder
        public async Task<IActionResult> Index()
        {
            return User.IsInRole("Teacher") ? View(await _folderService.GetFoldersByTeacherId(_userManager.FindByNameAsync(User.Identity.Name).Result.Id, true)) : View(_context.Folders.Where(x => !x.ParentId.HasValue).Include(x =>x.Lessons).OrderBy(x => x.Order).AsQueryable());
        }

        // GET: Folder/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // GET: Folder/Create
        public IActionResult Create(long? parentId, long? teacherId)
        {
            if (parentId.HasValue)
                ViewData["ParentId"] = parentId;
            else
                ViewData["ParentList"] = new SelectList(_context.Folders, "Id", "Name");
            if (teacherId.HasValue)
                ViewData["TeacherId"] = teacherId;
            else
                ViewData["TeacherList"] = new SelectList(_userService.GetAllTeachers(), "Id", "UserName");
            return View();
        }

        // POST: Folder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description,ParentId,TeacherId,Order,Price,IsActive")] Folder folder)
        {
            if (ModelState.IsValid)
            {
				folder.CreatedAt = DateTime.Now;
                _context.Add(folder);
                await _context.SaveChangesAsync(); 
                return folder.ParentId.HasValue ? RedirectToAction(nameof(Manage), new { id = folder.ParentId }) : RedirectToAction(nameof(Index));

            }
            return View(folder);
        }

        // GET: Folder/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders.FindAsync(id);
            if (folder == null)
            {
                return NotFound();
            }
            ViewData["ParentList"] = new SelectList(_context.Folders.Where(x => x.Id != id), "Id", "Name", folder.ParentId);
            ViewData["TeacherList"] = new SelectList(_userService.GetAllTeachers(), "Id", "UserName",folder.TeacherId);
            return View(folder);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,TeacherId,ParentId,Order,Price,IsActive")] Folder folder)
        {
            if (id != folder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(folder);
                await _context.SaveChangesAsync();
                return folder.ParentId.HasValue ? RedirectToAction(nameof(Manage), new {id = folder.ParentId}) : RedirectToAction(nameof(Index));
            }
            return View(folder);
        }

        // GET: Folder/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // POST: Folder/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var folder = await _context.Folders.FindAsync(id);
            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Manage(long id)
        {
            return View(await _context.Folders.FindAsync(id));
        }

        public async Task<IActionResult> ApprovedUsers(long id)
        {
            ViewData["FolderId"] = id;
            return View(await _folderService.GetApprovedUsers(id));
        }

        public IActionResult ApproveUser(long folderId, string users)
        {
            var userList = users.Split(',');
            foreach (var userEmail in userList)
            {
                var user =  _userManager.FindByEmailAsync(userEmail.Trim()).Result;
                if (user == null)
                    continue;
                _folderService.ApproveUser(folderId, user.Id);
            }
            return RedirectToAction(nameof(Manage), new {id = folderId});
        }

        public IActionResult DeleteApprovedUser(long folderId, long userId)
        {
            _folderService.DeleteUser(folderId, userId);
            return RedirectToAction(nameof(Manage), new { id = folderId });
        }
    }
}
