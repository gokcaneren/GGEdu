using GGEdu.Core.Enums;

namespace GGEdu.Core.DTOs.Teachers.Output
{
    public class TeacherCourseOutputDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public Currencies Currency { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
