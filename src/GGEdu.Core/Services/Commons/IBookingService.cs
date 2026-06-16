using GGEdu.Core.DTOs.Courses.Bookings.Input;
using GGEdu.Core.DTOs.Courses.Bookings.Output;
using GGEdu.Core.DTOs.Courses.TeacherCourses.Output;
using GGEdu.Core.Enums;
using GGEdu.Core.Utilities;

namespace GGEdu.Core.Services.Commons
{
    public interface IBookingService
    {
        Task<ApiResponse<TeacherCourseWithCourseSlotOutputDto>> GetTeacherWithSlotsAsync(
            Guid teacherId,
            string courseCode,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> SendBookingRequestAsync(
            BookingInputDto bookingInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<PagedResponse<BookingRequestOutputDto>>> GetBookingRequestsAsync(
            BookingStatus? status,
            int? nextDays,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> DecideBookingRequestAsync(
            DecideBookRequestInputDto decideBookRequestInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> CancelBookingRequestAsStudentAsync(
            Guid teacherId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> CancelBookingRequestAsTeacherAsync(
            Guid studentId,
            CancellationToken cancellationToken = default);
    }
}
