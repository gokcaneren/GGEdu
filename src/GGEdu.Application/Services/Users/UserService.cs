using AutoMapper;
using GGEdu.Application.Utilities;
using GGEdu.Core.DTOs.Emails.Input;
using GGEdu.Core.DTOs.Users.Inputs;
using GGEdu.Core.DTOs.Users.Outputs;
using GGEdu.Core.Entities.Students;
using GGEdu.Core.Entities.Teachers;
using GGEdu.Core.Entities.Users;
using GGEdu.Core.Enums;
using GGEdu.Core.Repositories.Users;
using GGEdu.Core.Services;
using GGEdu.Core.Services.Users;
using GGEdu.Core.UnitOfWorks;
using GGEdu.Core.Utilities;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace GGEdu.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IAuthService _authService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResources> _localizer;

        public UserService(
            IUserRepository userRepository,
            IAuthService authService,
            IStringLocalizer<SharedResources> localizer,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IEmailNotificationService emailNotificationService)
        {
            _userRepository = userRepository;
            _authService = authService;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _emailNotificationService = emailNotificationService;
        }

        public async Task<ApiResponse<UserProfileOutputDto>> GetProfileAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

                if (user == null)
                {
                    return ApiResponse<UserProfileOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Auth.UserCantBeFound"], null);
                }

                var userProfileOutputDto = _mapper.Map<UserProfileOutputDto>(user);

                return ApiResponse<UserProfileOutputDto>.SuccessResponse(HttpStatusCode.OK, _localizer["Gnrl.Success"], userProfileOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> RegisterAsync(
            UserRegisterInputDto userRegisterInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                //TODO: Control will changed with NormalizedEmail!!
                var user = await _userRepository.GetByAsync(c=>c.Email.Equals(userRegisterInputDto.Email), cancellationToken);

                if (user != null)
                {
                    return ApiResponse<bool>.ErrorResponse(HttpStatusCode.BadRequest, _localizer["Auth.UserAlreadyExistError"], false);
                }

                var newUser = new User()
                {
                    Email = userRegisterInputDto.Email,
                    NormalizedEmail = userRegisterInputDto.Email.ToLower().Trim(),
                    PasswordHash = PasswordManager.Hash(userRegisterInputDto.Password),
                    FirstName = userRegisterInputDto.FirstName,
                    LastName = userRegisterInputDto.LastName,
                    Gender = userRegisterInputDto.Gender
                };


                if (userRegisterInputDto.IsTeacher)
                {
                    newUser.Teacher = new Teacher();
                }
                else
                {
                    newUser.Student = new Student();
                }

                newUser.EmailVerificationToken = CreateEmailVerificationToken();
                newUser.EmailVerificationSentAt = DateTime.UtcNow;

                await _userRepository.CreateAsync(newUser, autoSave: false, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                _ = Task.Run(async () =>
                    await _emailNotificationService.SendEmailVerificationMail(
                        new EmailVerificationInputDto
                        {
                            Email = newUser.Email,
                            FirstName = newUser.FirstName,
                            Token = newUser.EmailVerificationToken
                        }),
                    CancellationToken.None);

                return ApiResponse<bool>.SuccessResponse(HttpStatusCode.Created, _localizer["Auth.UserCreated"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string CreateEmailVerificationToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
        }

        public async Task<ApiResponse<UserSignInOutputDto>> SignInAsync(
            UserSignInInputDto userSignInInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userRepository.GetByAsync(c => c.Email.Equals(userSignInInputDto.Email), cancellationToken);

                if (user == null)
                {
                    return ApiResponse<UserSignInOutputDto>.ErrorResponse(
                        HttpStatusCode.Unauthorized, _localizer["Auth.EmailOrPasswordError"], null);
                }

                if (!PasswordManager.Verify(userSignInInputDto.Password, user.PasswordHash))
                {
                    return ApiResponse<UserSignInOutputDto>.ErrorResponse(
                        HttpStatusCode.Unauthorized, _localizer["Auth.EmailOrPasswordError"], null);
                }

                await _userRepository.LoadNavigationPropertyAsync(user, c => c.Teacher);

                var userSignInOutput = new UserSignInOutputDto()
                {
                    Id = user.Id,
                    Token = _authService.GenerateToken(user),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Teacher != null ? UserRoles.Teacher : UserRoles.Student
                };

                return ApiResponse<UserSignInOutputDto>.SuccessResponse(HttpStatusCode.OK, _localizer["Gnrl.Success"], userSignInOutput);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> VerifyEmailAsync(
            string token,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userRepository.GetByAsync(c => c.EmailVerificationToken.Equals(token), cancellationToken);

                if (user == null)
                {
                    return false;
                }

                if (user.EmailConfirmed)
                {
                    return false;
                }

                if (user.EmailVerificationSentAt < DateTime.UtcNow.AddHours(-24))
                {
                    return false;
                }

                user.EmailConfirmed = true;
                user.EmailVerificationToken = null;

                await _unitOfWork.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
