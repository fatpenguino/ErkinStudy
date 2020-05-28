using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Entities.Payment;
using ErkinStudy.Domain.Enums;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Infrastructure.DTOs;
using ErkinStudy.Infrastructure.ExternalServices;
using ErkinStudy.Web.Models;
using ErkinStudy.Web.Models.Payment;
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
        public async Task<IActionResult> CreateOrder(long folderId, string email, string phoneNumber, long amount)
        {
            var folder = await _dbContext.Folders.FirstOrDefaultAsync(x => x.Id == folderId);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (folder != null && user != null)
            {
                var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(x => x.FolderId == folder.Id && x.UserId == user.Id);
                if (existingOrder != null)
                    RedirectToAction("NewOrder");
                var order = new Order
                {
                    FolderId = folder.Id,
                    Amount = amount,
                    Email = email,
                    UserId = user.Id,
                    PhoneNumber = phoneNumber,
                    CreatedDate = DateTime.Now,
                    OrderStatus = OrderStatus.Created,
                    ExpireDate = DateTime.Now.AddMinutes(30)
                };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
                var paymentResponse = await _wooppayPaymentService.Payment(new OrderRequestDto() {Amount = amount, OrderId = order.Id, PhoneNumber = phoneNumber, Email = email});
                if (paymentResponse != null)
                {
                    return RedirectToAction("Payment", new { operationUrl = paymentResponse.OperationUrl });
                }
            }
            return RedirectToAction("Error", "Home");
        }

        [Authorize]
        public async Task<IActionResult> NewOrder(long folderId)
        {
            var folder = await _dbContext.Folders.FirstOrDefaultAsync(x => x.Id == folderId);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (folder == null || user == null) return View();
            {
                var existingOrder =
                    await _dbContext.Orders.FirstOrDefaultAsync(x => x.FolderId == folder.Id && x.UserId == user.Id);
                if (existingOrder != null)
                    return View();
                var model = new NewOrderViewModel
                {
                    FolderId = folderId,
                    UserId = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Amount = folder.Price
                };
                return View(model);
            }
        }

        [Authorize]
        public IActionResult Payment(PaymentViewModel model)
        {
            return View(model);
        }
    }
}