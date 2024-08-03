namespace WebApplication1.Models
{
    public class Course:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public int Duration { get; set; }
        public int ClassDuration { get; set; }
        public int SkillLevelId { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int AssessmentId { get; set; }
        public Assessment Assessment { get; set; }
        public int MaxStudents { get; set; }
        public int Fee { get; set; }
        public string ImageUrl { get; set; }
    }
}
