namespace GGEdu.Core.DTOs.Teachers.Output
{
    public class TeacherDetailOutputDto
    {
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public int DurationMinutes { get; set; }
        public string TimeZoneId { get; set; }
        public Guid UserId { get; set; }
        public List<TeacherCourseOutputDto>? TeacherCourses { get; set; }
    }
}
