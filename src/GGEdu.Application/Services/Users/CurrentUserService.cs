using GGEdu.Core.Services.Users;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GGEdu.Application.Services.Users
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid UserId { get; }

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            if (accessor.HttpContext?.User.Identity.IsAuthenticated != false)
            {
                var user = accessor.HttpContext?.User;

                var claim = user?.FindFirst(ClaimTypes.NameIdentifier);

                if (claim == null)
                    throw new UnauthorizedAccessException();

                UserId = Guid.Parse(claim.Value);
            }
        }
    }
}
