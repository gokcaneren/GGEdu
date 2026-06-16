using GGEdu.Core.Entities.Commons.Bookings;
using GGEdu.Core.Repositories.Commons;
using GGEdu.Infrastructure.Context;

namespace GGEdu.Infrastructure.Repositories.Commons
{
    public class BookingRequestRepository : GenericRepository<BookingRequest>, IBookingRequestRepository
    {
        public BookingRequestRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }
    }
}
