using System;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Entities.Payment;
using ErkinStudy.Domain.Enums;
using ErkinStudy.Infrastructure.Context;
using ErkinStudy.Infrastructure.DTOs;
using ErkinStudy.Infrastructure.ExternalServices;
using ErkinStudy.Infrastructure.Services;
using ErkinStudy.Web.Models;
using ErkinStudy.Web.Models.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WooppayPaymentService _wooppayPaymentService;
        private readonly OrderService _orderService;
        private readonly FolderService _folderService;
        public PaymentController(AppDbContext dbContext, ILogger<PaymentController> logger, UserManager<ApplicationUser> userManager, WooppayPaymentService wooppayPaymentService, OrderService orderService, FolderService folderService)
        {
            _logger = logger;
            _userManager = userManager;
            _wooppayPaymentService = wooppayPaymentService;
            _orderService = orderService;
            _folderService = folderService;
        }

        [Authorize]
        public async Task<IActionResult> CreateOrder(long folderId, string email, string phoneNumber, long amount)
        {
            var folder = _folderService.Get(folderId);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (folder != null && user != null)
            {
                var existingOrder = _orderService.GetByUserIdAndFolderId(folderId, user.Id);
                if (existingOrder != null && existingOrder.ExpireDate > DateTime.Now)
                    RedirectToAction("Index","Home");
                var order = _orderService.CreateOrder(folderId, user.Id, email, phoneNumber, folder.Price);
                await _orderService.LogOperation(order.Id, $"Был создан новый заказ, {order.Id}");
                var paymentResponse = await _wooppayPaymentService.Payment(new OrderRequestDto() {Amount = amount, OrderId = order.Id, PhoneNumber = phoneNumber, Email = email});
                if (paymentResponse != null && paymentResponse.ErrorCode == 0)
                {
                    await _orderService.SetExternalId(order.Id, paymentResponse.OperationUrl);
                    await _orderService.ChangeStatus(order.Id, OrderStatus.SentToPay);
                    return RedirectToAction("Payment", new { operationUrl = paymentResponse.OperationUrl });
                }
                {
                    await _orderService.ChangeStatus(order.Id, OrderStatus.Declined);
                }
            }
            return RedirectToAction("Error", "Home");
        }

        [Authorize]
        public async Task<IActionResult> NewOrder(long folderId)
        {
            var folder = _folderService.Get(folderId);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (folder == null || user == null) return View();
            {
                var existingOrder = _orderService.GetByUserIdAndFolderId(folderId, user.Id);
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

        public IActionResult SuccessPage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmPayment(string orderId, string hash)
        {
            try
            {
                _logger.LogDebug($"Пришел запрос от wooppay, {orderId}, {hash}");
                var parsedOrder = long.Parse(orderId);
                var newHash = _orderService.GetHash(parsedOrder);
                if (newHash == hash)
                {
                   await _orderService.ConfirmOrder(parsedOrder);
                   return Ok(new {data = 1});
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");
            }
            return StatusCode(500);
        }
    }
}