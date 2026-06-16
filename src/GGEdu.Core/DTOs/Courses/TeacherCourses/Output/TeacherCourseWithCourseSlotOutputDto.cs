using GGEdu.Core.DTOs.Courses.AvailabilityCourseSlot.Input;
using GGEdu.Core.DTOs.Courses.AvailabilityCourseSlot.Output;
using GGEdu.Core.Enums;

namespace GGEdu.Core.DTOs.Courses.TeacherCourses.Output
{
    public class TeacherCourseWithCourseSlotOutputDto
    {
        public Guid TeacherCourseId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public Currencies Currency { get; set; }
        public List<AvailabilityCourseSlotOutputDto> CourseSlots { get; set; }
    }
}
