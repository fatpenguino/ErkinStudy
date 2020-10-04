using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Helpers;
using ErkinStudy.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FolderService _folderService;
        public ProfileController(UserManager<ApplicationUser> userManager, FolderService folderService)
        {
            this._userManager = userManager;
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
            user.PhoneNumber = UtilHelper.RemoveInputMaskFromPhoneNumber(phone);
            await _userManager.UpdateAsync(user);
            TempData["SuccessMessage"] = "Профиль cәтті өзгертілді";
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
