using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Notice:BaseEntity
    {
        public DateOnly Date { get; set; }
        [StringLength(200)]
        public string Content { get; set; }
    }
}
