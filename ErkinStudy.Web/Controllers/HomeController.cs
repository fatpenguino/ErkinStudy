using System.Diagnostics;
using System.Threading.Tasks;
using ErkinStudy.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ErkinStudy.Web.Models;

namespace ErkinStudy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SubjectService _subjectService;
        public HomeController(ILogger<HomeController> logger, SubjectService subjectService)
        {
            _logger = logger;
            _subjectService = subjectService;
        }

        public async Task<IActionResult> Index()
        {
            var subjects = await _subjectService.GetAllAsync();
            return View(subjects);
        }

        public IActionResult Privacy() 
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
