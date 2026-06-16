namespace GGEdu.Core.DTOs.Courses.CourseTemplates.Output
{
    public class CourseTemplateWithCourseOutputDto
    {
        public Guid Id { get; set; }
        public Guid TeacherCourseId { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartLocalTime { get; set; }
        public TimeSpan EndLocalTime { get; set; }
        public string TimeZoneId { get; set; }
        public bool AutoGenerateSlots { get; set; }
        public int GenerateDaysAhead { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }
    }
}
