using GGEdu.Core.Entities.Commons.Subscriptions;
using GGEdu.Core.Enums;

namespace GGEdu.Core.Repositories.Commons
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<(List<Subscription> Items, int TotalCount)> GetSubscribersBySubscriptionStatusAsync(
        Guid userId,
        SubscriptionStatus subscriptionStatus,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    }
}
