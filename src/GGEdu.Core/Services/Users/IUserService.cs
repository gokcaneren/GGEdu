using GGEdu.Core.DTOs.Users.Inputs;
using GGEdu.Core.DTOs.Users.Outputs;
using GGEdu.Core.Utilities;

namespace GGEdu.Core.Services.Users
{
    public interface IUserService
    {
        Task<ApiResponse<UserSignInOutputDto>> SignInAsync(
            UserSignInInputDto userSignInInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> RegisterAsync(
            UserRegisterInputDto userRegisterInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<UserProfileOutputDto>> GetProfileAsync(
            CancellationToken cancellationToken = default);

        Task<bool> VerifyEmailAsync(
            string token,
            CancellationToken cancellationToken = default);
    }
}
