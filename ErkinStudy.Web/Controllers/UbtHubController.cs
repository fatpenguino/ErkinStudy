using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Services;
using ErkinStudy.Web.Models.UbtHub;
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

        public  IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetSpecialties([FromBody] GetSpecialtiesViewModel model)
        {
            var result = await _specialtyService.GetSpecialties(-1, -1);
            return Json(result.Select(x => new { x.Id, x.Title}) );
        }
    }
}
