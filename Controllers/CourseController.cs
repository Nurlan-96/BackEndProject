using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
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
                return PartialView("_SearchPartialView", new List<Course>());
            }

            var courses = _context.Courses
                .Where(b =>
                    b.Name.ToLower().Contains(text.ToLower()) ||
                    b.Description.ToLower().Contains(text.ToLower()))
                .OrderByDescending(b => b.Id)
                .Take(3)
                .ToList();

            return PartialView("_SearchPartialView", courses);
        }
    }
}
