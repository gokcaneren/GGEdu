namespace GGEdu.Core.DTOs.Subscriptions.Output
{
    public class SubbedTeacherOutputDto
    {
        public Guid TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? DisplayName { get; set; }
        public string Photo { get; set; }
        public bool Gender { get; set; }
    }
}
