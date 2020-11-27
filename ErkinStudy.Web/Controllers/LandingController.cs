using Microsoft.AspNetCore.Mvc;

namespace ErkinStudy.Web.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
