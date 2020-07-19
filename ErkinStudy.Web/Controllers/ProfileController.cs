using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProfileController> _logger;
        public ProfileController(UserManager<ApplicationUser> userManager, ILogger<ProfileController> logger)
        {
            this._userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }

    }
}
