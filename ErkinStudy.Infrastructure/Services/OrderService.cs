using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ErkinStudy.Domain.Enums;
using ErkinStudy.Infrastructure.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.Lessons;
using ErkinStudy.Domain.Entities.Payment;
using ErkinStudy.Web.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Infrastructure.Services
{
    public class OrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly AppDbContext _dbContext;

        public OrderService(AppDbContext dbContext, ILogger<OrderService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public Order GetByUserIdAndFolderId(long folderId, long userId)
        {
            return _dbContext.Orders.FirstOrDefault(x => x.FolderId == folderId && x.UserId == userId);
        }

        public async Task<List<Order>> GetAll(int count)
        {
            return await _dbContext.Orders.Include(x => x.User).Include(x => x.Folder).OrderByDescending(x => x.Id).Take(count).ToListAsync();
        }
        public async Task<Order> Get(long orderId)
        {
            return await _dbContext.Orders
                .Include(x => x.Folder).Include(x => x.User)
                .Include(x => x.OrderOperations).FirstOrDefaultAsync(x => x.Id == orderId);
        }
        public Order CreateOrder(long folderId, long userId, string email, string phoneNumber, long amount)
        {
            var order = new Order
            {
                FolderId = folderId,
                Amount = amount,
                Email = email,
                UserId = userId,
                PhoneNumber = phoneNumber,
                CreatedDate = DateTime.Now,
                OrderStatus = OrderStatus.Created,
                ExpireDate = DateTime.Now.AddMinutes(30)
            };
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
            return order;
        }
        public async Task ConfirmOrder(long orderId)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
                if (order != null)
                {
                    order.OrderStatus = OrderStatus.Confirmed;
                    order.ConfirmedDate = DateTime.Now;
                    // фиксируем транзакцию
                    var payment = new Payment
                    {
                        Amount = order.Amount, FolderId = order.FolderId, OrderId = order.Id, UserId = order.UserId
                    };
                    _dbContext.Payments.Add(payment);
                    // добавляем юзеру его курс
                    var userFolder = new UserFolder {UserId = order.UserId, FolderId = order.FolderId};
                    await _dbContext.UserFolders.AddAsync(userFolder);
                    await _dbContext.SaveChangesAsync();
                }
                else 
                    _logger.LogError($"Не нашли заказ по этому id - {orderId}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Ошибка во время подтверждений заказа, {e}");
            }
        }
        public async Task ChangeStatus(long orderId, OrderStatus status)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            order.OrderStatus = status;
            await _dbContext.SaveChangesAsync();
        }

        public string GetHash(long orderId)
        {
            var hash = Encoding.ASCII.GetBytes($"fatpenguino_{orderId}");
            var md5 = new MD5CryptoServiceProvider();
            var hashEnc = md5.ComputeHash(hash);
            return hashEnc.Aggregate("", (current, b) => current + b.ToString("x2"));
        }

        public async Task SetExternalId(long orderId, string externalId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            order.ExternalId = externalId;
            await _dbContext.SaveChangesAsync();
        }

        public async Task LogOperation(long orderId, string message)
        {
            var operation = new OrderOperation {OrderId = orderId, Message = message, TraceTime = TimeZoneHelper.GetLocalDateTime() };
            await _dbContext.OrderOperations.AddAsync(operation);
            await _dbContext.SaveChangesAsync();
        }
    }
}