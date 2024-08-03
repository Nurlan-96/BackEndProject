using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;

namespace WebApplication1.ViewComponents
{
    public class TeacherViewComponents:ViewComponent
    {
        private readonly AppDbContext _context;

        public TeacherViewComponents(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? take)
        {
            var teachers = _context.Teachers.Take(take ?? _context.Teachers.Count()).ToList();
            return View(await Task.FromResult(teachers));
        }
    }
}
