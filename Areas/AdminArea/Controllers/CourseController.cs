using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.AdminArea.ViewModels;
using WebApplication1.Areas.AdminArea.ViewModels.Courses;
using WebApplication1.DAL;
using WebApplication1.Extensions;
using WebApplication1.HelperMethods;
using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        public CourseController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var courses = _context.Courses.AsNoTracking().ToList();
            return View(courses);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM ccvm)
        {
            var languages = _context.Languages.ToList();
            ViewBag.Languages = new SelectList(languages, "Id", "Name");
            var assesments = _context.Assessments.ToList();
            ViewBag.Assessments = new SelectList(assesments, "Id", "Name");
            var skilllevels = _context.SkillLevels.ToList();
            ViewBag.SkillLevels = new SelectList(skilllevels, "Id", "Name");
            var teachers = _context.Teachers.ToList();
            ViewBag.Teachers = new SelectList(teachers, "Id", "Name");
            string imageUrl = null;
            if (ccvm.Image != null)
            {
                var imageDirectory = "wwwroot/img/course";

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                var fileName = Path.GetFileNameWithoutExtension(ccvm.Image.FileName);
                var extension = Path.GetExtension(ccvm.Image.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(imageDirectory, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ccvm.Image.CopyToAsync(fileStream);
                }
                imageUrl = $"~/img/course/{uniqueFileName}";
            }

            var newccvm = new Course()
            {
                Name = ccvm.Name,
                Description = ccvm.Description,
                StartDate = ccvm.StartDate,
                Duration = ccvm.Duration,
                ClassDuration = ccvm.ClassDuration,
                Teacher = ccvm.Teacher,
                Language = ccvm.Language,
                SkillLevel = ccvm.SkillLevel,
                Fee = ccvm.Fee,
                Assessment = ccvm.Assessment,
                MaxStudents = ccvm.MaxStudents,
                ImageUrl = imageUrl,
            };
            await _context.Courses.AddAsync(newccvm);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var course = await _context.Courses.FirstOrDefaultAsync(t => t.Id == id);
            if (course == null) return NotFound();
            var cuVM = new CourseUpdateVM
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                StartDate = course.StartDate,
                Duration = course.Duration,
                ClassDuration = course.ClassDuration,
                Teacher = course.Teacher,
                Language = course.Language,
                SkillLevel = course.SkillLevel,
                Fee = course.Fee,
                MaxStudents = course.MaxStudents,
                ImageUrl = course.ImageUrl
            };
            return View(cuVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, CourseUpdateVM cuVM)
        {
            if (id == null) return BadRequest();
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null) return NotFound();
            var file = cuVM.Image;
            if (file == null)
            {
                ModelState.AddModelError("Image", "Can't be empty");
                cuVM.ImageUrl = course.ImageUrl;
                return View(cuVM);
            }
            string fileName = await file.SaveFile();
            Helper.DeleteImage(course.ImageUrl);
            cuVM.ImageUrl = fileName;
            course.Name = cuVM.Name;
            course.Description = cuVM.Description;
            course.StartDate = cuVM.StartDate;
            course.Duration = cuVM.Duration;
            course.Teacher = cuVM.Teacher;
            course.Language = cuVM.Language;
            course.SkillLevel = cuVM.SkillLevel;
            course.Assessment = cuVM.Assessment;
            course.MaxStudents = cuVM.MaxStudents;
            course.Fee = cuVM.Fee;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();

            if (!string.IsNullOrEmpty(course.ImageUrl))
            {
                string imageUrl = course.ImageUrl.StartsWith("~") ? course.ImageUrl.Substring(1) : course.ImageUrl;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
