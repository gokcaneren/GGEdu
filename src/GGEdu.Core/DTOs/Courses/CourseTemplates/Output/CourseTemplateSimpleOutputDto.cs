namespace GGEdu.Core.DTOs.Courses.CourseTemplates.Output
{
    public class CourseTemplateSimpleOutputDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; }
        public string Name { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartLocalTime { get; set; }
        public TimeSpan EndLocalTime { get; set; }
    }
}
