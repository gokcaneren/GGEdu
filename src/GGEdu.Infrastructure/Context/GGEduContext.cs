using GGEdu.Core.Entities.Commons.Bookings;
using GGEdu.Core.Entities.Commons.Subscriptions;
using GGEdu.Core.Entities.Students;
using GGEdu.Core.Entities.Teachers;
using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Entities.Teachers.Languages;
using GGEdu.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GGEdu.Infrastructure.Context
{
    public class GGEduContext : DbContext
    {
        public GGEduContext(DbContextOptions<GGEduContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<TeacherCourse> TeacherCourses { get; set; }
        public DbSet<CourseTemplate> CourseTemplates { get; set; }
        public DbSet<AvailabilityCourseSlot> AvailabilityCourseSlots { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<TeacherLanguage> TeacherLanguages { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingRequest> BookingRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
