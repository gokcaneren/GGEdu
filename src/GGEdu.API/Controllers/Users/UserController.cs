using GGEdu.Core.DTOs.Users.Inputs;
using GGEdu.Core.DTOs.Users.Outputs;
using GGEdu.Core.Services.Users;
using GGEdu.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GGEdu.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ApiResponse<UserProfileOutputDto>> GetProfileAsync(
            CancellationToken cancellationToken = default)
        {
            return await _userService.GetProfileAsync(cancellationToken);
        }

        [HttpPost("register")]
        public async Task<ApiResponse<bool>> RegisterAsync(
            UserRegisterInputDto userRegisterInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _userService.RegisterAsync(userRegisterInputDto, cancellationToken);
        }

        [HttpPost("signin")]
        public async Task<ApiResponse<UserSignInOutputDto>> SignInAsync(
            [FromBody]UserSignInInputDto userSignInInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _userService.SignInAsync(userSignInInputDto, cancellationToken);
        }

        //TODO: This method will be changed.
        [HttpGet("verify/{token}")]
        public async Task<IActionResult> VerifyEmailAsync(
        string token,
        CancellationToken cancellationToken = default)
        {
            var result = await _userService.VerifyEmailAsync(token, cancellationToken);

            return result ? Redirect("http://localhost:3000/tr/login?verified=true")
                : BadRequest();
        }
    }
}
