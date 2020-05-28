using ErkinStudy.Domain.Enums;
using ErkinStudy.Infrastructure.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
        public async Task ChangeStatus(long orderId, OrderStatus status)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            order.OrderStatus = status;
            await _dbContext.SaveChangesAsync();
        }
    }
}
