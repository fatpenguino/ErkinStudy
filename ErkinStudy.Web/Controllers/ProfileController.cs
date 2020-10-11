using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Helpers;
using ErkinStudy.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ErkinStudy.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FolderService _folderService;
        public ProfileController(UserManager<ApplicationUser> userManager, FolderService folderService)
        {
            _userManager = userManager;
            _folderService = folderService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Courses()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var folders = await _folderService.GetUserFolders(user.Id);
            return View(folders);
        }
    }
}
