using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var homeVM = new HomeVM()
            {
                Sliders = _context.Sliders.AsNoTracking().ToList(),
                Courses = _context.Courses.AsNoTracking().ToList(),
                Events = _context.Events.AsNoTracking().ToList(),
                Blogs = _context.Blogs.AsNoTracking().ToList(),
            };
            return View(homeVM);
        }
    }
}
