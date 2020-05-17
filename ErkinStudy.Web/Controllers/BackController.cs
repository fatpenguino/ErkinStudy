using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    [Authorize]
    public class BackController : Controller
    {
        private readonly ILogger<BackController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public BackController(ILogger<BackController> logger, AppDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            return View(currentUser);
        }

        public async Task<IActionResult> UserInfo()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            return View(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> UserInfo([Bind("FirstName,LastName,Email,PhoneNumber")] ApplicationUser user)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Email = user.Email;
                currentUser.PhoneNumber = user.PhoneNumber;

                _dbContext.Users.Update(currentUser);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(currentUser);
        }
    }
}