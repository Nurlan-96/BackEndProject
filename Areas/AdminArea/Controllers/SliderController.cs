using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.AdminArea.ViewModels.Slider;
using WebApplication1.DAL;
using WebApplication1.Extensions;
using WebApplication1.Helper;
using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var sliders = _context.Sliders.AsNoTracking().ToList();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM sliderCreateVM)
        {
            var file = sliderCreateVM.Image;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "Can't be empty");
                return View(sliderCreateVM);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photo", "Invalid file type");
                return View(sliderCreateVM);
            }
            if (file.CheckSize(1000))
            {
                ModelState.AddModelError("Photo", "Exceeds the maxsimum size");
                return View(sliderCreateVM);
            }

            Slider slider = new Slider();
            slider.ImageUrl = await file.SaveFile();
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var slider = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null) return NotFound();
            Helper.Helper.DeleteImage(slider.ImageUrl);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
