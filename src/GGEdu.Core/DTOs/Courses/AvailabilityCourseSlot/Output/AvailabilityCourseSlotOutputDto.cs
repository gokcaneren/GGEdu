using GGEdu.Core.Enums;

namespace GGEdu.Core.DTOs.Courses.AvailabilityCourseSlot.Output
{
    public class AvailabilityCourseSlotOutputDto
    {
        public Guid Id { get; set; }
        public DateTime StartAtUtc { get; set; }
        public DateTime EndAtUtc { get; set; }
        public AvailabilityCourseSlotStatus Status { get; set; }
        public Guid CourseTemplateId { get; set; }
    }
}
