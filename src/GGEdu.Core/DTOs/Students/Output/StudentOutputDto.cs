namespace GGEdu.Core.DTOs.Students.Output
{
    public class StudentOutputDto
    {
        public Guid Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public Guid UserId { get; set; }
    }
}
