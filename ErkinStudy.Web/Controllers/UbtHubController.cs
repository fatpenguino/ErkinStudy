using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErkinStudy.Web.Controllers
{
    public class UbtHubController : Controller
    {
        private readonly SpecialtyService _specialtyService;
        private readonly ILogger<UbtHubController> _logger;
        public UbtHubController(SpecialtyService specialtyService, ILogger<UbtHubController> logger)
        {
            _specialtyService = specialtyService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _specialtyService.GetAll());
        }
    }
}
