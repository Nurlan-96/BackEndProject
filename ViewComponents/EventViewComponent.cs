using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;

namespace WebApplication1.ViewComponents
{
    public class EventViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;

        public EventViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? take)
        {
            var events = _context.Events.Take(take ?? _context.Events.Count()).ToList();
            return View(await Task.FromResult(events));
        }
    }
}
