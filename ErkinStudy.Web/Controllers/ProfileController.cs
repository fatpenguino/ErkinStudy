using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Services;
using ErkinStudy.Web.Controllers.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProfileController> _logger;
        private readonly FolderService _folderService;
        public ProfileController(UserManager<ApplicationUser> userManager, ILogger<ProfileController> logger, FolderService folderService)
        {
            this._userManager = userManager;
            _logger = logger;
            _folderService = folderService;
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string firstname, string lastname, string phone)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.FirstName = firstname;
            user.LastName = lastname;
            user.PhoneNumber = phone;
            await _userManager.UpdateAsync(user);
            TempData["SuccessMessage"] = "Edit success";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Courses()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var folders = await _folderService.GetUserFolders(user.Id);
            return View(folders);
        }
    }
}
