using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class LanguageController : Controller
    {
        private readonly AppDbContext _context;

        public LanguageController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var languages = _context.Languages.AsNoTracking().ToList(); 
            return View(languages);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(string name, Language language)
        {
            Language newLanguage = new Language();
            newLanguage.Name = name;
            await _context.Languages.AddAsync(newLanguage);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var language = await _context.Languages.FirstOrDefaultAsync(c => c.Id == id);
            if (language == null) return NotFound();
            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
