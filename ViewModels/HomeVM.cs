using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Notice> Notices { get; set; }
    }
}
