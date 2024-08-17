using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var teachers = _context.Teachers.ToList();
            return View(teachers);
        }
        public IActionResult Detail(int? id)
        {
            if (id is null) return NotFound();
            var teacher = _context.Teachers.AsNoTracking().FirstOrDefault(t => t.Id == id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }
    }
}
