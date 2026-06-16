using AutoMapper;
using GGEdu.Core.DTOs.Courses.AvailabilityCourseSlot.Output;
using GGEdu.Core.DTOs.Courses.Bookings.Input;
using GGEdu.Core.DTOs.Courses.Bookings.Output;
using GGEdu.Core.DTOs.Courses.TeacherCourses.Output;
using GGEdu.Core.DTOs.Subscriptions.Output;
using GGEdu.Core.Entities.Commons.Bookings;
using GGEdu.Core.Entities.Teachers;
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
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAvailabilityCourseSlotRepository _availabilityCourseSlotRepository;

        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResources> _localizer;

        public BookingService(
            IBookingRepository bookingRepository,
            ITeacherRepository teacherRepository,
            ISubscriptionRepository subscriptionRepository,
            IStudentRepository studentRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            IAvailabilityCourseSlotRepository availabilityCourseSlotRepository)
        {
            _bookingRepository = bookingRepository;
            _teacherRepository = teacherRepository;
            _subscriptionRepository = subscriptionRepository;
            _studentRepository = studentRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _availabilityCourseSlotRepository = availabilityCourseSlotRepository;
        }

        public async Task<ApiResponse<bool>> CancelBookingRequestAsStudentAsync(
            Guid teacherId,
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

                var booking = await _bookingRepository.GetByAsync(c =>
                c.StudentId.Equals(student.Id) && c.TeacherId.Equals(teacherId) &&
                (c.Status == BookingStatus.Pending || c.Status == BookingStatus.Scheduled), cancellationToken);

                if (booking == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Bk.BookingsCantBeFoundError"], false);
                }

                booking.Status = BookingStatus.Cancelled;
                booking.CancelledDate = DateTime.UtcNow;

                var availableCourseSlot = await _availabilityCourseSlotRepository.GetByAsync(c =>
                c.Id.Equals(booking.AvailabilityCourseSlotId) && c.TeacherId.Equals(teacherId)
                && c.Status == AvailabilityCourseSlotStatus.Pending);

                if (availableCourseSlot == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Slot.NotAvailableSlotError"], false);
                }

                availableCourseSlot.Status = AvailabilityCourseSlotStatus.Available;

                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> CancelBookingRequestAsTeacherAsync(
            Guid studentId,
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

                var booking = await _bookingRepository.GetByAsync(c =>
                c.StudentId.Equals(studentId) && c.TeacherId.Equals(teacher.Id) &&
                (c.Status == BookingStatus.Pending || c.Status == BookingStatus.Scheduled), cancellationToken);

                if (booking == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Bk.BookingsCantBeFoundError"], false);
                }

                booking.Status = BookingStatus.Cancelled;
                booking.CancelledDate = DateTime.UtcNow;

                var availableCourseSlot = await _availabilityCourseSlotRepository.GetByAsync(c =>
                c.Id.Equals(booking.AvailabilityCourseSlotId) && c.TeacherId.Equals(teacher.Id)
                && c.Status == AvailabilityCourseSlotStatus.Pending);

                if (availableCourseSlot == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Slot.NotAvailableSlotError"], false);
                }

                availableCourseSlot.Status = AvailabilityCourseSlotStatus.Available;

                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> DecideBookingRequestAsync(
            DecideBookRequestInputDto decideBookRequestInputDto,
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

                var booking = await _bookingRepository.GetByAsync(c =>
                c.TeacherId.Equals(teacher.Id) && c.StudentId.Equals(decideBookRequestInputDto.StudentId) &&
                c.Id.Equals(decideBookRequestInputDto.Id) &&
                c.Status == BookingStatus.Pending, cancellationToken);

                if (booking == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Bk.StudentBookıngCantBeFoundError"], false);
                }

                booking.Status = decideBookRequestInputDto.IsAccepted == true
                    ? BookingStatus.Scheduled
                    : BookingStatus.Rejected;
                booking.DecisionDate = DateTime.UtcNow;

                var availableCourseSlot = await _availabilityCourseSlotRepository.GetByAsync(c =>
                c.Id.Equals(booking.AvailabilityCourseSlotId) && c.TeacherId.Equals(teacher.Id)
                && c.Status == AvailabilityCourseSlotStatus.Pending);

                if (availableCourseSlot == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Slot.NotAvailableSlotError"], false);
                }

                availableCourseSlot.Status = booking.Status == BookingStatus.Scheduled
                    ? AvailabilityCourseSlotStatus.Booked
                    : AvailabilityCourseSlotStatus.Available;

                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<PagedResponse<BookingRequestOutputDto>>> GetBookingRequestsAsync(
            BookingStatus? status,
            int? nextDays,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (teacher == null)
                {
                    //TODO: Buradakı hata mesajının sonuna error eklenecek!
                    return ApiResponse<PagedResponse<BookingRequestOutputDto>>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.TeacherCantBeFound"], null);
                }

                var (bookingRequestList, totalCount) = await _bookingRepository.GetBookingsByTeacherIdAndByStatusAsnyc(
                    teacher.Id, status, nextDays, page, pageSize, cancellationToken);

                if (bookingRequestList.IsNullOrEmpty())
                {
                    return ApiResponse<PagedResponse<BookingRequestOutputDto>>.SuccessResponse(
                        HttpStatusCode.OK, _localizer["Bk.BookingsCantBeFoundError"], null);
                }

                var bookingRequestOutputDtos = bookingRequestList.Select(c => new BookingRequestOutputDto
                {
                    Id = c.Id,
                    StudentId = c.StudentId,
                    AvailabilityCourseSlotId = c.AvailabilityCourseSlotId,
                    FirstName = c.Student.User.FirstName,
                    LastName = c.Student.User.LastName,
                    Email = c.Student.User.Email,
                    Photo = c.Student.User.Photo,
                    CourseId = c.AvailabilityCourseSlot.TeacherCourse.Course.Id,
                    CourseName = c.AvailabilityCourseSlot.TeacherCourse.Course.Name,
                    CourseCode = c.AvailabilityCourseSlot.TeacherCourse.Course.Code,
                    CourseStartDate = c.AvailabilityCourseSlot.StartAtUtc,
                    CourseEndDate = c.AvailabilityCourseSlot.EndAtUtc
                }).ToList();

                var pagedResponse = PagedResponse<BookingRequestOutputDto>.CreatePagedResponse(
                    bookingRequestOutputDtos, totalCount, page, pageSize);

                return ApiResponse<PagedResponse<BookingRequestOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], pagedResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<TeacherCourseWithCourseSlotOutputDto>> GetTeacherWithSlotsAsync(
            Guid teacherId,
            string courseCode,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var sturdent = await _studentRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (sturdent == null)
                {
                    return ApiResponse<TeacherCourseWithCourseSlotOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Stdt.StudentCantBeFound"], null);
                }

                var isTeacherSubbed = await _subscriptionRepository.AnyAsync(c =>
                c.TeacherId.Equals(teacherId) && c.StudentId.Equals(sturdent.Id) &&
                c.Status == SubscriptionStatus.Accepted);

                if (isTeacherSubbed == false)
                {
                    return ApiResponse<TeacherCourseWithCourseSlotOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Sub.NotSubbedTeacherError"], null);
                }

                var teacherSlots = await _availabilityCourseSlotRepository.GetTeacherCourseSlotByTeacherIdAndCourseCodeAsync(
                    teacherId, courseCode, cancellationToken);

                if (teacherSlots.IsNullOrEmpty())
                {
                    return ApiResponse<TeacherCourseWithCourseSlotOutputDto>.ErrorResponse(
                        HttpStatusCode.NotFound, _localizer["Slot.NotAvailableSlotError"], null);
                }

                var firstSlot = teacherSlots.First();

                var teacherCourseWithCourseSlotOutputDto = new TeacherCourseWithCourseSlotOutputDto
                {
                    TeacherCourseId = firstSlot.TeacherCourseId,
                    CourseId = firstSlot.TeacherCourse.Course.Id,
                    CourseName = firstSlot.TeacherCourse.Course.Name,
                    CourseCode = firstSlot.TeacherCourse.Course.Code,
                    Currency = firstSlot.TeacherCourse.Currency,
                    Price = firstSlot.TeacherCourse.Price,
                    DurationMinutes = firstSlot.TeacherCourse.DurationMinutes,
                    CourseSlots = teacherSlots.Select(c => new AvailabilityCourseSlotOutputDto
                    {
                        CourseTemplateId = c.CourseTemplateId,
                        Id = c.Id,
                        StartAtUtc = c.StartAtUtc,
                        EndAtUtc = c.EndAtUtc,
                        Status = c.Status
                    }).OrderBy(c => c.StartAtUtc).ToList()
                };

                return ApiResponse<TeacherCourseWithCourseSlotOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], teacherCourseWithCourseSlotOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> SendBookingRequestAsync(
            BookingInputDto bookingInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var sturdent = await _studentRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (sturdent == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Stdt.StudentCantBeFound"], false);
                }

                var isTeacherSubbed = await _subscriptionRepository.AnyAsync(c =>
                c.TeacherId.Equals(bookingInputDto.TeacherId) && c.StudentId.Equals(sturdent.Id) &&
                c.Status == SubscriptionStatus.Accepted);

                if (isTeacherSubbed == false)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Sub.NotSubbedTeacherError"], false);
                }

                var availableCourseSlot = await _availabilityCourseSlotRepository.GetByAsync(c =>
                c.Id.Equals(bookingInputDto.AvailabilityCourseSlotId) && c.TeacherId.Equals(bookingInputDto.TeacherId)
                && c.Status == AvailabilityCourseSlotStatus.Available);

                if (availableCourseSlot == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Slot.NotAvailableSlotError"], false);
                }

                availableCourseSlot.Status = AvailabilityCourseSlotStatus.Pending;

                var newBooking = new Booking
                {
                    StudentId = sturdent.Id,
                    TeacherId = bookingInputDto.TeacherId,
                    AvailabilityCourseSlotId = bookingInputDto.AvailabilityCourseSlotId,
                    Status = BookingStatus.Pending
                };

                await _bookingRepository.CreateAsync(newBooking, autoSave: false, cancellationToken);
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
