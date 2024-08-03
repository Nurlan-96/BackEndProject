using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;

namespace WebApplication1.ViewComponents
{
    public class CourseViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public CourseViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? take)
        {
            var courses = _context.Courses.Take(take ?? _context.Courses.Count()).ToList();
            return View(await Task.FromResult(courses));
        }
    }
}
