namespace WebApplication1.Models
{
    public class Speaker:BaseEntity
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<SpeakerEvent> SpeakerEvent { get; set; }
    }
}
