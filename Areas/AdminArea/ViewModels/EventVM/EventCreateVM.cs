using WebApplication1.Models;

namespace WebApplication1.Areas.AdminArea.ViewModels.EventVM
{
    public class EventCreateVM:Event
    {
        public IFormFile Image { get; set; }
        public List<int> SelectedSpeakerIds { get; set; }
    }
}
