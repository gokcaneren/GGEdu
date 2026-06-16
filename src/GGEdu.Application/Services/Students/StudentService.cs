using AutoMapper;
using GGEdu.Core.DTOs.Students.Input;
using GGEdu.Core.DTOs.Students.Output;
using GGEdu.Core.DTOs.Teachers.Output;
using GGEdu.Core.Repositories.Students;
using GGEdu.Core.Repositories.Users;
using GGEdu.Core.Services.Students;
using GGEdu.Core.Services.Users;
using GGEdu.Core.UnitOfWorks;
using GGEdu.Core.Utilities;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace GGEdu.Application.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;

        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResources> _localizer;

        public StudentService(
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService,
            IUserRepository userRepository)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<StudentOutputDto>> GetProfileAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

                if (user == null)
                {
                    return ApiResponse<StudentOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Auth.UserCantBeFound"], null);
                }

                var student = await _studentRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (student == null)
                {
                    return ApiResponse<StudentOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Stdt.NotAssignedAsStudent"], null);
                }

                var studentOutputDto = _mapper.Map<StudentOutputDto>(student);

                return ApiResponse<StudentOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], studentOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> UpdateProfileAsync(
            StudentProfileUpdateInputDto studentProfileInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

                if (user == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Auth.UserCantBeFound"], false);
                }

                var student = await _studentRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (student == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Stdt.NotAssignedAsStudent"], false);
                }

                student.DisplayName = studentProfileInputDto.DisplayName;
                student.Bio = studentProfileInputDto.Bio;

                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
