using AutoMapper;
using GGEdu.Core.DTOs.Subscriptions.Input;
using GGEdu.Core.DTOs.Subscriptions.Output;
using GGEdu.Core.Entities.Commons.Subscriptions;
using GGEdu.Core.Enums;
using GGEdu.Core.Repositories.Commons;
using GGEdu.Core.Repositories.Students;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Core.Services.Commons;
using GGEdu.Core.Services.Users;
using GGEdu.Core.UnitOfWorks;
using GGEdu.Core.Utilities;
using GGEdu.Infrastructure.Extensions;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace GGEdu.Application.Services.Commons
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;

        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResources> _localizer;

        public SubscriptionService(
            ISubscriptionRepository subscriptionRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer,
            IMapper mapper,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _mapper = mapper;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
        }

        public async Task<ApiResponse<bool>> DecideSubRequestAsync(
            SubRequestDecideInputDto subRequestDecideInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.TeacherCantBeFound"], false);
                }

                var subRequest = await _subscriptionRepository.GetByAsync(
                    c => c.TeacherId.Equals(teacher.Id) && c.StudentId.Equals(subRequestDecideInputDto.StudentId), cancellationToken);

                if(subRequest == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Sub.ThereIsNoRequestError"], false);
                }

                if (subRequest.Status != SubscriptionStatus.Requested)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Sub.ThereIsNoRequestError"], false);
                }

                subRequest.Status = subRequestDecideInputDto.IsAccepted ? SubscriptionStatus.Accepted : SubscriptionStatus.Rejected;
                subRequest.DecisionDate = DateTime.UtcNow;

                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<List<SubbedTeacherOutputDto>>> GetSubbedTeachersAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var student = await _studentRepository.GetSubbedTeachersByUserIdAsync(userId, cancellationToken);

                if (student == null)
                {
                    return ApiResponse<List<SubbedTeacherOutputDto>>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Stdt.StudentCantBeFound"], null);
                }

                var subbedTeacherOutputDtos = student.Subscriptions.Select(c=>
                {
                    return new SubbedTeacherOutputDto
                    {
                        TeacherId = c.TeacherId,
                        DisplayName = c.Teacher.DisplayName,
                        FirstName = c.Teacher.User.FirstName,
                        LastName = c.Teacher.User.LastName,
                        Photo = c.Teacher.User.Photo,
                        Gender = c.Teacher.User.Gender
                    };
                }).ToList();

                return ApiResponse<List<SubbedTeacherOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], subbedTeacherOutputDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<PagedResponse<SubscriberOutputDto>>> GetSubscribersBySubscriptionStatusAsync(
            SubscriptionStatus subscriptionStatus,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var (subscribers, totalCount) = await _subscriptionRepository.GetSubscribersBySubscriptionStatusAsync(
                    userId, subscriptionStatus, page, pageSize, cancellationToken);

                if (subscribers.IsNullOrEmpty())
                {
                    return ApiResponse<PagedResponse<SubscriberOutputDto>>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Sub.SubscribersCantBeFound"], null);
                }

                var subscribersOutputDtos = subscribers.Select(c =>
                {
                    return new SubscriberOutputDto
                    {
                        StudentId = c.StudentId,
                        DisplayName = c.Student.DisplayName,
                        FirstName = c.Student.User.FirstName,
                        LastName = c.Student.User.LastName,
                        Photo = c.Student.User.Photo
                    };
                }).ToList();

                var pagedResponse = PagedResponse<SubscriberOutputDto>.CreatePagedResponse(
                    subscribersOutputDtos, totalCount, page, pageSize);

                return ApiResponse<PagedResponse<SubscriberOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], pagedResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> SendSubRequestAsync(
            SubRequestInputDto subRequestInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var student = await _studentRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (student == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Stdt.StudentCantBeFound"], false);
                }

                var isAlreadyExist = await _subscriptionRepository.GetByAsync(
                    c => c.TeacherId.Equals(subRequestInputDto.TeacherId) && c.StudentId.Equals(student.Id), cancellationToken);

                if (isAlreadyExist == null)
                {
                    var subRequest = new Subscription
                    {
                        StudentId = student.Id,
                        TeacherId = subRequestInputDto.TeacherId,
                        Status = SubscriptionStatus.Requested
                    };

                    await _subscriptionRepository.CreateAsync(subRequest, autoSave: false, cancellationToken);

                    await _unitOfWork.CommitAsync(cancellationToken);

                    return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], true);
                }

                if (isAlreadyExist.Status == SubscriptionStatus.Accepted || isAlreadyExist.Status == SubscriptionStatus.Requested)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Sub.RequestAlreadySentOrAcceptedError"], false);
                }

                isAlreadyExist.Status = SubscriptionStatus.Requested;

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
