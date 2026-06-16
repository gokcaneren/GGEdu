using GGEdu.Core.Entities.Students;
using GGEdu.Core.Entities.Teachers;
using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Enums;

namespace GGEdu.Core.Entities.Commons.Bookings
{
    //NOTE: Bu entity iptal edildi. Sadece booking kullanılması yeterli görüldü.
    public class BookingRequest : BaseEntity
    {
        public BookingRequestStatus Status { get; set; }
        public DateTime? DecisionDate { get; set; }

        public Guid TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public Guid StudentId { get; set; }
        public virtual Student Student { get; set; }

        public Guid AvailabilityCourseSlotId { get; set; }
        public virtual AvailabilityCourseSlot AvailabilityCourseSlot { get; set; }
    }
}
