using GGEdu.Core.Entities.Teachers;
using GGEdu.Core.Enums;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GGEdu.Infrastructure.Repositories.Teachers
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }

        public async Task<Teacher?> GetSubscribersBySubscriptionStatusAsync(
            Guid userId,
            SubscriptionStatus subscriptionStatus,
            CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Teachers
                .Where(c => c.UserId.Equals(userId))
                .Include(c => c.Subscribers.Where(x => x.Status == subscriptionStatus))
                .ThenInclude(c => c.Student)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Teacher?> GetTeacherDetailByTeacherIdAsync(
            Guid teacherId,
            CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Teachers
                .Where(c => c.Id.Equals(teacherId))
                .Include(c => c.TeacherCourses)
                .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Teacher?> GetTeacherDetailByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Teachers
                .Where(c => c.UserId.Equals(userId))
                .Include(c => c.TeacherCourses)
                .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Teacher?> GetTeacherWithLanguagesByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Teachers
                .Where(c => c.UserId.Equals(userId))
                .Include(c => c.TeacherLanguages)
                .ThenInclude(c => c.Language)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Teacher?> GetTeacherWithTeacherCoursesByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Teachers
                .Where(c => c.UserId.Equals(userId))
                .Include(c => c.TeacherCourses)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
