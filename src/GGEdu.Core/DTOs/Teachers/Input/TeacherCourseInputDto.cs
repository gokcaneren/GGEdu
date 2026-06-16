using GGEdu.Core.Enums;

namespace GGEdu.Core.DTOs.Teachers.Input
{
    public class TeacherCourseInputDto
    {
        public Guid? Id { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public Currencies Currency { get; set; }
        public Guid CourseId { get; set; }
    }
}
