namespace GGEdu.Core.DTOs.Teachers.Output
{
    public class TeacherOutputDto
    {
        public Guid Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public int DurationMinutes { get; set; }
        public string TimeZoneId { get; set; }
        public Guid UserId { get; set; }
    }
}
