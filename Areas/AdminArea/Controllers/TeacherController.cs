using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.AdminArea.ViewModels;
using WebApplication1.DAL;
using WebApplication1.Extensions;
using WebApplication1.Helper;
using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        public TeacherController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var teachers = _context.Teachers.AsNoTracking().ToList();
            return View(teachers);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var teacher = await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(TeacherCreateVM tcvm)
        {
            string imageUrl = null;
            if (tcvm.Image != null)
            {
                var imageDirectory = "wwwroot/img/teacher";

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                var fileName = Path.GetFileNameWithoutExtension(tcvm.Image.FileName);
                var extension = Path.GetExtension(tcvm.Image.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(imageDirectory, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await tcvm.Image.CopyToAsync(fileStream);
                }
                imageUrl = $"~/img/teacher/{uniqueFileName}";
            }

            var newtcvm = new Teacher()
            {
                Name = tcvm.Name,
                Description = tcvm.Description,
                Position = tcvm.Position,
                Faculty = tcvm.Faculty,
                Degree = tcvm.Degree,
                Email = tcvm.Email,
                Number = tcvm.Number,
                Experience = tcvm.Experience,
                Social = tcvm.Social,
                Hobbies = tcvm.Hobbies,
                ImageUrl = imageUrl,
            };
            await _context.Teachers.AddAsync(newtcvm);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return NotFound();
            var teacherUpdateVM = new TeacherUpdateVM
            {
                Id = teacher.Id,
                Name = teacher.Name,
                Description = teacher.Description,
                Position = teacher.Position,
                Faculty = teacher.Faculty,
                Degree = teacher.Degree,
                Number = teacher.Number,
                Email = teacher.Email,
                Experience = teacher.Experience,
                Social = teacher.Social,
                Hobbies = teacher.Hobbies,
                ImageUrl = teacher.ImageUrl
            };
            return View(teacherUpdateVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, TeacherUpdateVM teacherUpdateVM)
        {
            if (id == null) return BadRequest();
            var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (teacher == null) return NotFound();
            var file = teacherUpdateVM.Image;
            if (file == null)
            {
                ModelState.AddModelError("Image", "Can't be empty");
                teacherUpdateVM.ImageUrl = teacher.ImageUrl;
                return View(teacherUpdateVM);
            }
            string fileName = await file.SaveFile();
            Helper.Helper.DeleteImage(teacher.ImageUrl);
            teacherUpdateVM.ImageUrl = fileName;
            teacher.Name = teacherUpdateVM.Name;
            teacher.Position = teacherUpdateVM.Position;
            teacher.Degree = teacherUpdateVM.Degree;
            teacher.Number = teacherUpdateVM.Number;
            teacher.Email = teacherUpdateVM.Email;
            teacher.Experience = teacherUpdateVM.Experience;
            teacher.Social = teacherUpdateVM.Social;
            teacher.Description = teacherUpdateVM.Description;
            teacher.Faculty = teacherUpdateVM.Faculty;
            teacher.Hobbies = teacherUpdateVM.Hobbies;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var teacher = await _context.Teachers.FirstOrDefaultAsync(c => c.Id == id);
            if (teacher == null) return NotFound();

            if (!string.IsNullOrEmpty(teacher.ImageUrl))
            {
                string imageUrl = teacher.ImageUrl.StartsWith("~") ? teacher.ImageUrl.Substring(1) : teacher.ImageUrl;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
