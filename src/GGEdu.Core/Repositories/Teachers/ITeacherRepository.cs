using GGEdu.Core.Entities.Teachers;
using GGEdu.Core.Enums;

namespace GGEdu.Core.Repositories.Teachers
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        Task<Teacher?> GetTeacherDetailByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<Teacher?> GetTeacherDetailByTeacherIdAsync(
            Guid teacherId,
            CancellationToken cancellationToken = default);

        Task<Teacher?> GetTeacherWithLanguagesByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<Teacher?> GetSubscribersBySubscriptionStatusAsync(
            Guid userId,
            SubscriptionStatus subscriptionStatus,
            CancellationToken cancellationToken = default);

        Task<Teacher?> GetTeacherWithTeacherCoursesByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
