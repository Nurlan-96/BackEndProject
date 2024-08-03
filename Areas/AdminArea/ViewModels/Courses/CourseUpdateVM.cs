using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.ViewModels.Courses
{
    public class CourseUpdateVM:Course
    {
        public IFormFile Image { get; set; }

    }
}
