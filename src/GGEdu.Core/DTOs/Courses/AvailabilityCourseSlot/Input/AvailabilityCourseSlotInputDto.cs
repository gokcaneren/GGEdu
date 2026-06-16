namespace GGEdu.Core.DTOs.Courses.AvailabilityCourseSlot.Input
{
    public class AvailabilityCourseSlotInputDto
    {
        public Guid CourseTemplateId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
    }
}
