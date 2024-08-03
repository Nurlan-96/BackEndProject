namespace WebApplication1.Models
{
    public class Blog:BaseEntity
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateOnly CreatedDate { get; set; }
    }
}
