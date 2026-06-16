using GGEdu.Core.DTOs.Subscriptions.Input;
using GGEdu.Core.DTOs.Subscriptions.Output;
using GGEdu.Core.Enums;
using GGEdu.Core.Utilities;

namespace GGEdu.Core.Services.Commons
{
    public interface ISubscriptionService
    {
        Task<ApiResponse<bool>> SendSubRequestAsync(
            SubRequestInputDto subRequestInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> DecideSubRequestAsync(
            SubRequestDecideInputDto subRequestDecideInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<PagedResponse<SubscriberOutputDto>>> GetSubscribersBySubscriptionStatusAsync(
            SubscriptionStatus subscriptionStatus,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<List<SubbedTeacherOutputDto>>> GetSubbedTeachersAsync(
            CancellationToken cancellationToken = default);
    }
}
