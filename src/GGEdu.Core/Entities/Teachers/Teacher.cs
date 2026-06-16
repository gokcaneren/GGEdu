using GGEdu.Core.Entities.Commons.Bookings;
using GGEdu.Core.Entities.Commons.Subscriptions;
using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Entities.Teachers.Languages;
using GGEdu.Core.Entities.Users;
using GGEdu.Core.Enums;

namespace GGEdu.Core.Entities.Teachers
{
    public class Teacher : BaseEntity
    {
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public int DurationMinutes { get; set; }
        public string TimeZoneId { get; set; } = "UTC";

        public Guid UserId { get; set; }
        public virtual User User{ get; set; }

        public virtual ICollection<TeacherCourse> TeacherCourses { get; set; }
        public virtual ICollection<CourseTemplate> CourseTemplates { get; set; }

        public virtual ICollection<AvailabilityCourseSlot> AvailabilityCourseSlots { get; set; }
        public virtual ICollection<TeacherLanguage> TeacherLanguages { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<BookingRequest> BookingRequests { get; set; }

        public virtual ICollection<Subscription> Subscribers { get; set; }
    }
}
