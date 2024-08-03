using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.ViewModels.EventVM
{
    public class EventUpdateVM:Event
    {
        public IFormFile Image { get; set; }
    }
}
