using GGEdu.Core.Enums;

namespace GGEdu.Core.DTOs.Teachers.Output
{
    public class TeacherCourseProfileOutputDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public Currencies Currency { get; set; }
    }
}
