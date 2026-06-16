using GGEdu.Core.Entities.Commons.Bookings;
using GGEdu.Core.Enums;

namespace GGEdu.Core.Entities.Teachers.Courses
{
    public class AvailabilityCourseSlot : BaseEntity
    {
        public DateTime StartAtUtc { get; set; }
        public DateTime EndAtUtc { get; set; }
        public AvailabilityCourseSlotStatus Status { get; set; }

        public Guid TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public Guid CourseTemplateId { get; set; }
        public virtual CourseTemplate CourseTemplate { get; set; }

        public Guid TeacherCourseId { get; set; }
        public virtual TeacherCourse TeacherCourse { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
