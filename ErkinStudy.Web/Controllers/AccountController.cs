using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            _logger.LogInformation($"Попытка входа пользователя {model.Username}.");
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Пользователь {model.Username} успешно вошел в сайт.");
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    _logger.LogInformation($"Пользователь {model.Username} залочен.");
                    return RedirectToAction(nameof(Lockout));
                }

                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                ModelState.AddModelError(string.Empty, message);
                _logger.LogError($"Ошибка при входе пользователя {model.Username}, {message}");
                return View(model);
            }

            var modelMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            _logger.LogError($"Ошибка при входе, кривые данные {model.Username}, {modelMessage}");
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            _logger.LogInformation($"Попытка регистраций пользователя {model.UserName}.");
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                    {Email = model.Email, UserName = model.UserName, PhoneNumber = model.PhoneNumber};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Тіркелу сәтті өтті.";
                    _logger.LogInformation($"Пользователь успешно зарегистрирован. { user.Id} - {user.UserName}");
                    return RedirectToAction("Login", "Account");
                }

                var message = string.Join(" | ", result.Errors
                    .Select(v => v.Description));
                ModelState.AddModelError(string.Empty, message);
                _logger.LogInformation($"Ошибка при регистраций пользователя {model.UserName}, {message}");
                return View(model);
            }
            var modelMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            _logger.LogError($"Ошибка при регистраций, кривые данные {modelMessage}");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation($"Пользователь {User.Identity.Name} успешно вышел");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Seed()
        {
            AppDbInitializer.SeedUsers(_userManager, _roleManager);
            return View(nameof(Login));
        }
    }
}