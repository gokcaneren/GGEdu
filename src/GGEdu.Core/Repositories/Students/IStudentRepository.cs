using GGEdu.Core.Entities.Students;

namespace GGEdu.Core.Repositories.Students
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Student?> GetSubbedTeachersByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
