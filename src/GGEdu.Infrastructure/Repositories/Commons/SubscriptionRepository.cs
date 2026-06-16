using GGEdu.Core.Entities.Commons.Subscriptions;
using GGEdu.Core.Enums;
using GGEdu.Core.Repositories.Commons;
using GGEdu.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GGEdu.Infrastructure.Repositories.Commons
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }

        public async Task<(List<Subscription> Items, int TotalCount)> GetSubscribersBySubscriptionStatusAsync(
            Guid userId,
            SubscriptionStatus subscriptionStatus,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _ggEduContext.Subscriptions
                .Where(c => c.Teacher.UserId.Equals(userId) && c.Status == subscriptionStatus)
                .Include(c => c.Student)
                    .ThenInclude(c => c.User)
                .OrderBy(c => c.Student.User.FirstName);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
