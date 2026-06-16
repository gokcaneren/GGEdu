namespace GGEdu.Core.DTOs.Courses.CourseTemplates.Input
{
    public class CourseTemplateInputDto
    {
        public Guid TeacherCourseId { get; set; }
        public string Name { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartLocalTime { get; set; }
        public TimeSpan EndLocalTime { get; set; }
        public string TimeZoneId { get; set; } = "UTC";
        public bool AutoGenerateSlots { get; set; }
        public int GenerateDaysAhead { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }
    }
}
