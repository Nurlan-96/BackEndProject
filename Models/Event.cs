namespace WebApplication1.Models
{
    public class Event:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Venue {  get; set; }
        public DateOnly Date {  get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ICollection<SpeakerEvent> SpeakerEvent { get; set; }
    }
}
