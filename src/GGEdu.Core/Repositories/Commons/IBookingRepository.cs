using GGEdu.Core.Entities.Commons.Bookings;
using GGEdu.Core.Enums;

namespace GGEdu.Core.Repositories.Commons
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<(List<Booking>, int totalCount)> GetBookingsByTeacherIdAndByStatusAsnyc(
            Guid teacherId,
            BookingStatus? bookingStatus,
            int? nextDays,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
