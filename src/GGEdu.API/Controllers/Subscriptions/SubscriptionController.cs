using GGEdu.Core.DTOs.Subscriptions.Input;
using GGEdu.Core.DTOs.Subscriptions.Output;
using GGEdu.Core.Enums;
using GGEdu.Core.Services.Commons;
using GGEdu.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GGEdu.API.Controllers.Subscriptions
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("request")]
        public async Task<ApiResponse<bool>> SendSubRequestAsync(
            SubRequestInputDto subRequestInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _subscriptionService.SendSubRequestAsync(subRequestInputDto, cancellationToken);
        }

        [HttpPost("decide")]
        public async Task<ApiResponse<bool>> DecideSubRequestAsync(
            SubRequestDecideInputDto subRequestDecideInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _subscriptionService.DecideSubRequestAsync(subRequestDecideInputDto, cancellationToken);
        }

        [HttpGet("{subscriptionStatus}/subscribers")]
        public async Task<ApiResponse<PagedResponse<SubscriberOutputDto>>> GetSubscribersBySubscriptionStatusAsync(
            SubscriptionStatus subscriptionStatus,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _subscriptionService.GetSubscribersBySubscriptionStatusAsync(subscriptionStatus, page, pageSize, cancellationToken);
        }

        [HttpGet("subbed/teachers")]
        public async Task<ApiResponse<List<SubbedTeacherOutputDto>>> GetSubbedTeachersAsync(
            CancellationToken cancellationToken = default)
        {
            return await _subscriptionService.GetSubbedTeachersAsync(cancellationToken);
        }
    }
}
