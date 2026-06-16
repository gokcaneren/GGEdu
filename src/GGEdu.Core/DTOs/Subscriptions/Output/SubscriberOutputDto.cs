namespace GGEdu.Core.DTOs.Subscriptions.Output
{
    public class SubscriberOutputDto
    {
        public Guid StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? DisplayName { get; set; }
        public string Photo { get; set; }
    }
}
