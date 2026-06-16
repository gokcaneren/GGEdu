using GGEdu.Core.Entities.Commons.Bookings;
using GGEdu.Core.Entities.Commons.Subscriptions;
using GGEdu.Core.Entities.Users;

namespace GGEdu.Core.Entities.Students
{
    public class Student : BaseEntity
    {
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<BookingRequest> BookingRequests { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
