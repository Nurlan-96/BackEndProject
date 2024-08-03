using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.AdminArea.ViewModels.Slider
{
    public class SliderUpdateVM
    {
        [Required]
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
    }
}
