using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            var courses = await _context.Courses.AsNoTracking().Skip((page-1)*2).Take(3).ToListAsync();
            var count = _context.Courses.Count();
            ViewBag.PageCount= (int)Math.Ceiling((decimal)count/3);
            ViewBag.CurrentPage = page;
            return View(courses);
        }
        public IActionResult Detail(int? id)
        {
            if (id is null) return NotFound();
            var courses = _context.Courses.AsNoTracking().FirstOrDefault(b => b.Id == id);
            if (courses == null) return NotFound();
            return View(courses);
        }
        public IActionResult Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest();
            }

            var courses = _context.Courses
                .Where(b =>
                    b.Name.ToLower().Contains(text.ToLower()) ||
                    b.Description.ToLower().Contains(text.ToLower()))
                .OrderByDescending(b => b.Id)
                .Take(3)
                .ToList();

            return PartialView("_CoursePartial", courses);
        }
    }
}
