using GGEdu.Core.Entities.Students;
using GGEdu.Core.Entities.Teachers;
using GGEdu.Core.Enums;

namespace GGEdu.Core.Entities.Commons.Subscriptions
{
    public class Subscription : BaseEntity
    {
        public SubscriptionStatus Status { get; set; }

        public DateTime? DecisionDate { get; set; }

        public Guid TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public Guid StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
