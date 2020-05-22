using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Entities.Payment;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PaymentController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WooppayPaymentService _wooppayPaymentService;
        public PaymentController(AppDbContext dbContext, ILogger<PaymentController> logger, UserManager<ApplicationUser> userManager, WooppayPaymentService wooppayPaymentService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userManager = userManager;
            _wooppayPaymentService = wooppayPaymentService;
        }

        [Authorize]
        public async Task<IActionResult> CreateOrder(long folderId)
        {
            var folder = await _dbContext.Folders.FirstOrDefaultAsync(x => x.Id == folderId);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (folder != null && user != null)
            {
                var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(x => x.FolderId == folder.Id && x.UserId == user.Id);
                if (existingOrder != null)
                    return View(existingOrder);
                var order = new Order
                {
                    FolderId = folder.Id,
                    Amount = folder.Price,
                    Email = user.Email,
                    UserId = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    CreatedTime = DateTime.Now
                };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
                var orderId = order.Id;
                return View(order);
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MakePayment(long orderId)
        {

            return View();
        }
    }
}