using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;

        public EventController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var events = _context.Events.ToList();
            return View(events);
        }
        public IActionResult Detail(int? id)
        {
            var @event = _context.Events
        .Include(e => e.SpeakerEvent)
            .ThenInclude(se => se.Speaker)
        .AsNoTracking()
        .FirstOrDefault(b => b.Id == id);
            return View(@event);
        }
    }
}
