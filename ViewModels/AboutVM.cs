using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class AboutVM
    {
        public IEnumerable<Notice> Notices { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
    }
}
