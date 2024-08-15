using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.AdminArea.ViewModels;
using WebApplication1.Areas.AdminArea.ViewModels.EventVM;
using WebApplication1.DAL;
using WebApplication1.Extensions;
using WebApplication1.Helper;
using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.Controllers
{
    public class SpeakerController : Controller
    {
        private readonly AppDbContext _context;
        public SpeakerController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var speakers = _context.Speakers.AsNoTracking().ToList();
            return View(speakers);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var speaker = await _context.Speakers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (speaker == null) return NotFound();
            return View(speaker);
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Create(SpeakerCreateVM scVM)
        {
            string imageUrl = null;
            if (scVM.Image != null)
            {
                var imageDirectory = "wwwroot/img/course";

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                var fileName = Path.GetFileNameWithoutExtension(scVM.Image.FileName);
                var extension = Path.GetExtension(scVM.Image.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(imageDirectory, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await scVM.Image.CopyToAsync(fileStream);
                }
                imageUrl = $"~/img/course/{uniqueFileName}";
            }

            var newscVM = new Event()
            {
                Name = scVM.Name,
                Description = scVM.Position,
                ImageUrl = imageUrl,
            };
            await _context.Events.AddAsync(newscVM);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var speaker = await _context.Speakers.FirstOrDefaultAsync(s => s.Id == id);
            if (speaker == null) return NotFound();
            var scVM = new EventUpdateVM
            {
                Id = speaker.Id,
                Name = speaker.Name,
                Description = speaker.Position,
                ImageUrl = speaker.ImageUrl
            };
            return View(scVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, SpeakerCreateVM scVM)
        {
            if (id == null) return BadRequest();
            var speaker = await _context.Speakers.FirstOrDefaultAsync(x => x.Id == id);
            if (speaker == null) return NotFound();
            var file = scVM.Image;
            if (file == null)
            {
                ModelState.AddModelError("Image", "Can't be empty");
                scVM.ImageUrl = speaker.ImageUrl;
                return View(scVM);
            }
            string fileName = await file.SaveFile();
            Helper.Helper.DeleteImage(speaker.ImageUrl);
            scVM.ImageUrl = fileName;
            speaker.Name = scVM.Name;
            speaker.Position = scVM.Position;
            speaker.ImageUrl = scVM.ImageUrl;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var speaker = await _context.Speakers.FirstOrDefaultAsync(c => c.Id == id);
            if (speaker == null) return NotFound();

            if (!string.IsNullOrEmpty(speaker.ImageUrl))
            {
                string imageUrl = speaker.ImageUrl.StartsWith("~") ? speaker.ImageUrl.Substring(1) : speaker.ImageUrl;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.Speakers.Remove(speaker);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}