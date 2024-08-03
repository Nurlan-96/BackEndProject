using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.AdminArea.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}
