using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.ViewModels
{
    public class TeacherUpdateVM:Teacher
    {
        public IFormFile Image { get; set; }
    }
}
