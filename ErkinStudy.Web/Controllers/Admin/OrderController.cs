using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ErkinStudy.Infrastructure.Services;

namespace ErkinStudy.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(int count = 100)
        {
            return View( await _orderService.GetAll(count));
        }

        public async Task<IActionResult> Details(long orderId)
        {
            return View(await _orderService.Get(orderId));    
        }
    }
}
