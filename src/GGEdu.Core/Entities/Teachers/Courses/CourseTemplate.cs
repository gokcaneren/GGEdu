namespace GGEdu.Core.Entities.Teachers.Courses
{
    public class CourseTemplate : BaseEntity
    {
        public string Name { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartLocalTime { get; set; }
        public TimeSpan EndLocalTime { get; set; }

        // Used to interpret StartLocalTime/EndLocalTime during slot generation.
        public string TimeZoneId { get; set; }

        // Optional bounds to limit generated slots.
        public DateOnly? EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }

        public bool AutoGenerateSlots { get; set; } 
        public int GenerateDaysAhead { get; set; } 

        public bool IsActive { get; set; } 

        public Guid TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public Guid TeacherCourseId { get; set; }
        public virtual TeacherCourse TeacherCourse { get; set; }
    }
}
