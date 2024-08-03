using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<SkillLevel> SkillLevels { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<SpeakerEvent> SpeakerEvents { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
