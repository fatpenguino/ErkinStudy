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
		    var course = _context.Degrees.Include(x => x.Subject).Include(x => x.Paragraphs).ThenInclude(x => x.Lessons).FirstOrDefault(x => x.Id == id && x.IsActive);
            return View(course);
        }
    }
}