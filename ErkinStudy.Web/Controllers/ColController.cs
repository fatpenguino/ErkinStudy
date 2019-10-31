using System.Threading.Tasks;
using ErkinStudy.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Web.Controllers
{
    public class ColController : Controller
    {
	    private readonly AppDbContext _context;

	    public ColController(AppDbContext context)
	    {
		    _context = context;
	    }

	    public IActionResult Detail(long id)
	    {
		    var lesson = _context.Lessons.Include(x => x.Contents).Include(x => x.Paragraph).FirstOrDefaultAsync(x => x.Id == id).Result;
            return View(lesson);
        }
    }
}