namespace GGEdu.Core.DTOs.Courses.Bookings.Input
{
    public class DecideBookRequestInputDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public bool IsAccepted { get; set; }
    }
}
