using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.AdminArea.ViewModels.Blogs;
using WebApplication1.DAL;
using WebApplication1.Extensions;
using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var blogs = _context.Blogs.AsNoTracking().ToList();
            return View(blogs);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var blog = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM bcVM)
        {
            string imageUrl = null;
            if (bcVM.Image != null)
            {
                var imageDirectory = "wwwroot/img/course";

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                var fileName = Path.GetFileNameWithoutExtension(bcVM.Image.FileName);
                var extension = Path.GetExtension(bcVM.Image.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(imageDirectory, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await bcVM.Image.CopyToAsync(fileStream);
                }
                imageUrl = $"~/img/course/{uniqueFileName}";
            }

            var newbcVM = new Blog()
            {
                Name = bcVM.Name,
                Content = bcVM.Content,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                ImageUrl = imageUrl,
            };
            await _context.Blogs.AddAsync(newbcVM);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var blog = await _context.Events.FirstOrDefaultAsync(t => t.Id == id);
            if (blog == null) return NotFound();
            var bcVM = new BlogCreateVM
            {
                Id = blog.Id,
                Name = blog.Name,
                Content = blog.Description,
            };
            return View(bcVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, BlogCreateVM bcVM)
        {
            if (id == null) return BadRequest();
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null) return NotFound();
            var file = bcVM.Image;
            if (file == null)
            {
                ModelState.AddModelError("Image", "Can't be empty");
                bcVM.ImageUrl = bcVM.ImageUrl;
                return View(bcVM);
            }
            string fileName = await file.SaveFile();
            Helper.Helper.DeleteImage(fileName);
            bcVM.ImageUrl = fileName;
            blog.Name = bcVM.Name;
            blog.Content = bcVM.Content;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var blog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);
            if (blog == null) return NotFound();

            if (!string.IsNullOrEmpty(blog.ImageUrl))
            {
                string imageUrl = blog.ImageUrl.StartsWith("~") ? blog.ImageUrl.Substring(1) : blog.ImageUrl;
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
