using GGEdu.Core.Entities.Commons.Bookings;
using GGEdu.Core.Enums;
using GGEdu.Core.Repositories.Commons;
using GGEdu.Infrastructure.Context;
using GGEdu.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GGEdu.Infrastructure.Repositories.Commons
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }

        public async Task<(List<Booking>, int totalCount)> GetBookingsByTeacherIdAndByStatusAsnyc(
            Guid teacherId,
            BookingStatus? bookingStatus,
            int? nextDays,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _ggEduContext.Bookings
                .WhereIf(bookingStatus != null, c => c.Status == bookingStatus)
                .WhereIf(nextDays != null, c => c.AvailabilityCourseSlot.StartAtUtc <= DateTime.UtcNow.AddDays(nextDays.Value))
                .Where(c => c.TeacherId.Equals(teacherId))
                .Include(c => c.AvailabilityCourseSlot)
                    .ThenInclude(c => c.TeacherCourse)
                    .ThenInclude(c => c.Course)
                .Include(c => c.Student)
                    .ThenInclude(c => c.User)
                .OrderBy(c => c.AvailabilityCourseSlot.StartAtUtc);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
