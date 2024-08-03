using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.ViewModels.Blogs
{
    public class BlogCreateVM:Blog
    {
        public IFormFile Image { get; set; }

    }
}
