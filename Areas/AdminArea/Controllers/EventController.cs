using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL;
using WebApplication1.Helper;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.AdminArea.ViewModels.EventVM;
using WebApplication1.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class EventController : Controller
    {

        private readonly AppDbContext _context;
        public EventController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var events = _context.Events.AsNoTracking().ToList();
            return View(events);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var @event = await _context.Events.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (@event == null) return NotFound();
            return View(@event);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(EventCreateVM ecVM)
        {
            var speakers = _context.Speakers.ToList();
            ViewBag.Speakers = new SelectList(speakers, "Id", "Name");
            string imageUrl = null;
            if (ecVM.Image != null)
            {
                var imageDirectory = "wwwroot/img/course";

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                var fileName = Path.GetFileNameWithoutExtension(ecVM.Image.FileName);
                var extension = Path.GetExtension(ecVM.Image.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(imageDirectory, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ecVM.Image.CopyToAsync(fileStream);
                }
                imageUrl = $"~/img/course/{uniqueFileName}";
            }

            var newecVM = new Event()
            {
                Name = ecVM.Name,
                Description = ecVM.Description,
                StartTime = ecVM.StartTime,
                Date = ecVM.Date,
                EndTime = ecVM.EndTime,
                Venue = ecVM.Venue,
                SpeakerEvent = ecVM.SpeakerEvent,
                ImageUrl = imageUrl,
            };
            if (ecVM.SelectedSpeakerIds != null && ecVM.SelectedSpeakerIds.Any())
            {
                newecVM.SpeakerEvent = ecVM.SelectedSpeakerIds.Select(speakerId => new SpeakerEvent
                {
                    SpeakerId = speakerId
                }).ToList();
            }
            await _context.Events.AddAsync(newecVM);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var @event = await _context.Events.FirstOrDefaultAsync(t => t.Id == id);
            if (@event == null) return NotFound();
            var ecVM = new EventUpdateVM
            {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                StartTime = @event.StartTime,
                Date = @event.Date,
                EndTime = @event.EndTime,
                Venue = @event.Venue,
                SpeakerEvent = @event.SpeakerEvent,
                ImageUrl = @event.ImageUrl
            };
            return View(ecVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, EventUpdateVM euVM)
        {
            if (id == null) return BadRequest();
            var @event = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (@event == null) return NotFound();
            var file = euVM.Image;
            if (file == null)
            {
                ModelState.AddModelError("Image", "Can't be empty");
                euVM.ImageUrl = @event.ImageUrl;
                return View(euVM);
            }
            string fileName = await file.SaveFile();
            Helper.Helper.DeleteImage(@event.ImageUrl);
            euVM.ImageUrl = fileName;
            @event.Name = euVM.Name;
            @event.Description = euVM.Description;
            @event.StartTime = euVM.StartTime;
            @event.Date = euVM.Date;
            @event.EndTime = euVM.EndTime;
            @event.Venue = euVM.Venue;
            @event.SpeakerEvent = euVM.SpeakerEvent;
            @event.ImageUrl = euVM.ImageUrl;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var @event = await _context.Events.FirstOrDefaultAsync(c => c.Id == id);
            if (@event == null) return NotFound();

            if (!string.IsNullOrEmpty(@event.ImageUrl))
            {
                string imageUrl = @event.ImageUrl.StartsWith("~") ? @event.ImageUrl.Substring(1) : @event.ImageUrl;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
