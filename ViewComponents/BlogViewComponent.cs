using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;

namespace WebApplication1.ViewComponents
{
    public class BlogViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public BlogViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? take)
        {
            var blogs = _context.Blogs.Take(take ?? _context.Blogs.Count()).ToList();
            return View(await Task.FromResult(blogs));
        }
    }
}
