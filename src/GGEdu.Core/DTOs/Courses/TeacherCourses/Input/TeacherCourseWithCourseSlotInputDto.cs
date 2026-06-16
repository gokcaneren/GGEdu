using GGEdu.Core.Enums;

namespace GGEdu.Core.DTOs.Courses.TeacherCourses.Input
{
    public class TeacherCourseWithCourseSlotInputDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public AvailabilityCourseSlotStatus SlotStatus { get; set; } = AvailabilityCourseSlotStatus.Available;
    }
}
