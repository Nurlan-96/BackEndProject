using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class SkillController : Controller
    {
        private readonly AppDbContext _context;

        public SkillController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var skill = _context.Languages.AsNoTracking().ToList();
            return View(skill);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(string name, SkillLevel skill)
        {
            SkillLevel newSkill = new SkillLevel();
            newSkill.Name = name;
            await _context.SkillLevels.AddAsync(newSkill);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var skill = await _context.SkillLevels.FirstOrDefaultAsync(c => c.Id == id);
            if (skill == null) return NotFound();
            _context.SkillLevels.Remove(skill);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
