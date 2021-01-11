using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Services;
using ErkinStudy.Web.Models.UbtHub;
using Microsoft.AspNetCore.Mvc;

namespace ErkinStudy.Web.Controllers
{
    public class UbtHubController : Controller
    {
        private readonly SpecialtyService _specialtyService;

        public UbtHubController(SpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        public  IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetSpecialties([FromBody] GetSpecialtiesViewModel model)
        {
            short.TryParse(model.FirstSubject, out var first);
            short.TryParse(model.SecondSubject, out var second);
            var universities = new List<int>();
            if (model.Universities.Count > 0)
            {
                universities = model.Universities.Select(int.Parse).ToList();
            }
            var result = await _specialtyService.GetSpecialties(first, second, universities);
            return Json(result.Select(x => new { x.Id, x.Title}) );
        }
        public async Task<IActionResult> Specialty(short id)
        {
            var specialty = await _specialtyService.GetSpecialty(id);
            return View(specialty);
        }
    }
}
