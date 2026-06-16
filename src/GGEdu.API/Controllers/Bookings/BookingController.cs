using GGEdu.Core.DTOs.Courses.Bookings.Input;
using GGEdu.Core.DTOs.Courses.Bookings.Output;
using GGEdu.Core.DTOs.Courses.TeacherCourses.Output;
using GGEdu.Core.Enums;
using GGEdu.Core.Services.Commons;
using GGEdu.Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GGEdu.API.Controllers.Bookings
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{teacherId}/slots/{courseCode}")]
        public async Task<ApiResponse<TeacherCourseWithCourseSlotOutputDto>> GetTeacherWithSlotsAsync(
            Guid teacherId,
            string courseCode,
            CancellationToken cancellationToken = default)
        {
            return await _bookingService.GetTeacherWithSlotsAsync(teacherId, courseCode, cancellationToken);
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> SendBookingRequestAsync(
            BookingInputDto bookingInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _bookingService.SendBookingRequestAsync(bookingInputDto, cancellationToken);
        }

        [HttpGet]
        public async Task<ApiResponse<PagedResponse<BookingRequestOutputDto>>> GetBookingRequestByStatusAsync(
            BookingStatus? bookingStatus,
            int? nextDays,
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _bookingService.GetBookingRequestsAsync(bookingStatus, nextDays, page, pageSize, cancellationToken);
        }

        [HttpPost("decide")]
        public async Task<ApiResponse<bool>> DecideBookingRequestAsync(
            DecideBookRequestInputDto decideBookRequestInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _bookingService.DecideBookingRequestAsync(decideBookRequestInputDto, cancellationToken);
        }

        [HttpPut("/teacher/{studentId}/cancel")]
        public async Task<ApiResponse<bool>> CancelBookingAsTeacherAsync(
            Guid studentId,
            CancellationToken cancellationToken = default)
        {
            return await _bookingService.CancelBookingRequestAsTeacherAsync(studentId, cancellationToken);
        }

        [HttpPut("/student/{teacherId}/cancel")]
        public async Task<ApiResponse<bool>> CancelBookingAsStudentAsync(
            Guid teacherId,
            CancellationToken cancellationToken = default)
        {
            return await _bookingService.CancelBookingRequestAsStudentAsync(teacherId, cancellationToken);
        }
    }
}
