using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Infrastructure.Services;
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
        private readonly EmailService _emailService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, ILogger<AccountController> logger, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailService = emailService;
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
            _logger.LogInformation($"Попытка входа пользователя {model.Email}.");
            if (model.Email.Contains('@'))
            {
                const string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                          @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                          @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                var regex = new Regex(emailRegex);
                if (!regex.IsMatch(model.Email))
                {
                    _logger.LogError($"Email не подходит по регексу {model.Email}");
                    ModelState.AddModelError("Email", "Email is not valid");
                }
            }
            if (ModelState.IsValid)
            {
                var userName = model.Email;
                if (userName.Contains('@'))
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        _logger.LogError("Пользователь с таким email не найден");
                        ModelState.AddModelError(string.Empty, "Пользователь с таким login не найден");
                        return View(model);
                    }
                    userName = user.UserName;
                }
                var result =
                    await _signInManager.PasswordSignInAsync(userName, model.Password, true, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Пользователь {userName} успешно вошел в сайт.");
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    _logger.LogInformation($"Пользователь {userName} залочен.");
                    return RedirectToAction(nameof(Lockout));
                }

                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                ModelState.AddModelError(string.Empty, message);
                _logger.LogError($"Ошибка при входе пользователя {model.Email}, {message}");
                return View(model);
            }

            var modelMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            _logger.LogError($"Ошибка при входе, кривые данные {model.Email}, {modelMessage}");
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
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Попытка восстановления пароля {model.Email}");
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogError($"Ошибка при сбросе восстановления, не найден пользователь по такому email {model.Email}");
                    return View("ForgotPasswordConfirmation");
                }
                try
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    // Потом переделать в нормальный url генератор.
                    var callbackUrl = $"https://erkinstudy.kz/Account/ResetPassword?userId={user.Id}&code={code}";
                    await _emailService.SendEmailAsync("Құпия сөзді қалпына келтіру",
                        $"Құпия сөзді қалпына келтіру үшін <a href='{callbackUrl}'> сілтемені </a> басыңыз.", model.Email);
                    _logger.LogInformation($"Отправляем письмо для восстановление для пользователя по ссылке {callbackUrl}");
                }
                catch (Exception e)
                {
                    _logger.LogError($"Ошибка при восстановление пароля для пользователя {model.Email}, {e}");
                }
                return View("ForgotPasswordConfirmation");
            }
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _logger.LogInformation($"Попытка сброса пароля для пользователя {model.Email} по коду {model.Code}");
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogError($"Ошибка при сбросе пароля, не найден пользователь по такому email {model.Email}");
                return View("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Пользователь {user.Email} успешно сбросил пароль.");
                return View("ResetPasswordConfirmation");
            }
            var modelMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            _logger.LogError($"Ошибка при сбросе пароля для пользователя {model.Email}, {modelMessage}");
            return View(model);
        }
    }
}