using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserOnlineCourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserOnlineCourseController> _logger;
        public UserOnlineCourseController(AppDbContext context, UserManager<ApplicationUser> userManager, ILogger<UserOnlineCourseController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: UserOnlineCourse
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserOnlineCourses.Include(u => u.OnlineCourse).Include(u => u.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserOnlineCourse/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOnlineCourse = await _context.UserOnlineCourses
                .Include(u => u.OnlineCourse)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.OnlineCourseId == id);
            if (userOnlineCourse == null)
            {
                return NotFound();
            }

            return View(userOnlineCourse);
        }

        // GET: UserOnlineCourse/Create
        public IActionResult Approve()
        {
            ViewData["OnlineCourseId"] = new SelectList(_context.OnlineCourses, "Id", "Name");
            return View();
        }

        // POST: UserOnlineCourse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Approve(string userList, long courseId)
        {
            _logger.LogInformation($"Начинаем потверждение пользователей: {userList}, для курса - {courseId}");
            var emails = userList.Split(',');
            foreach (var email in emails)
            {
                try
                {
                    var formattedEmail = email.Replace(" ", "").Replace(@"\t","").Replace(@"\n","");
                    var user = await _userManager.FindByEmailAsync(formattedEmail);
                    if (user == null) continue;
                    var userOnlineCourse = new UserOnlineCourse { UserId = user.Id, OnlineCourseId = courseId };
                    _context.UserOnlineCourses.Add(userOnlineCourse);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Ошибка во время подтверждение пользователя {email}, курс - {courseId}, {e}");
                }
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation("Заканчиваем потверждение пользователей");
            ViewData["OnlineCourseId"] = new SelectList(_context.OnlineCourses, "Id", "Id", courseId);
            return RedirectToAction(nameof(Approve));
        }

        // GET: UserOnlineCourse/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOnlineCourse = await _context.UserOnlineCourses.FindAsync(id);
            if (userOnlineCourse == null)
            {
                return NotFound();
            }
            ViewData["OnlineCourseId"] = new SelectList(_context.OnlineCourses, "Id", "Id", userOnlineCourse.OnlineCourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOnlineCourse.UserId);
            return View(userOnlineCourse);
        }

        // POST: UserOnlineCourse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(long id, [Bind("UserId,OnlineCourseId,IsActive")] UserOnlineCourse userOnlineCourse)
        {
            if (id != userOnlineCourse.OnlineCourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userOnlineCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserOnlineCourseExists(userOnlineCourse.OnlineCourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OnlineCourseId"] = new SelectList(_context.OnlineCourses, "Id", "Id", userOnlineCourse.OnlineCourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userOnlineCourse.UserId);
            return View(userOnlineCourse);
        }
        
        public async Task<IActionResult> Delete(long userId, long courseId)
        {
            var userOnlineCourse = await _context.UserOnlineCourses
                .Include(u => u.OnlineCourse)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.OnlineCourseId == courseId && m.UserId == userId);
            if (userOnlineCourse == null)
            {
                return NotFound();
            }

            return View(userOnlineCourse);
        }

        // POST: UserOnlineCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long userId, long onlineCourseId)
        {
            var userOnlineCourse = await _context.UserOnlineCourses.FirstOrDefaultAsync(x => x.UserId == userId && x.OnlineCourseId == onlineCourseId);
            _context.UserOnlineCourses.Remove(userOnlineCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserOnlineCourseExists(long id)
        {
            return _context.UserOnlineCourses.Any(e => e.OnlineCourseId == id);
        }
    }
}
