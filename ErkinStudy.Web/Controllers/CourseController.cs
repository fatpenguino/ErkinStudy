using System.Linq;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Web.Controllers
{
    public class CourseController : Controller
    {
	    private readonly AppDbContext _context;

	    public CourseController(AppDbContext context)
	    {
		    _context = context;
	    }

	    public IActionResult Detail(long id)
        {
            return View();
        }
    }
}