using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;

namespace WebApplication1.ViewComponents
{
    public class NoticeViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public NoticeViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? take)
        {
            var notices = _context.Notices.Take(take ?? _context.Notices.Count()).ToList();
            return View(await Task.FromResult(notices));
        }
    }
}
