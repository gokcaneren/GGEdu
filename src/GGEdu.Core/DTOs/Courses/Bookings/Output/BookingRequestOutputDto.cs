namespace GGEdu.Core.DTOs.Courses.Bookings.Output
{
    public class BookingRequestOutputDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid AvailabilityCourseSlotId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime? CourseStartDate { get; set; }
        public DateTime? CourseEndDate { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
    }
}
