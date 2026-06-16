using GGEdu.Core.Entities.Students;
using GGEdu.Core.Repositories.Students;
using GGEdu.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GGEdu.Infrastructure.Repositories.Students
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }

        public async Task<Student?> GetSubbedTeachersByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Students
                .Where(c => c.UserId.Equals(userId))
                .Include(c => c.Subscriptions)
                .ThenInclude(c => c.Teacher)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
