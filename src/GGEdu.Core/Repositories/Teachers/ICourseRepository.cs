using GGEdu.Core.Entities.Teachers.Courses;

namespace GGEdu.Core.Repositories.Teachers
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<Course?> GetCourseWithTeachersByCourseCodeAsync(
            string courseCode,
            CancellationToken cancellationToken = default);
    }
}
