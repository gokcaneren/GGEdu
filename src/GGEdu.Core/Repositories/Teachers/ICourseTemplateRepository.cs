using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Utilities;

namespace GGEdu.Core.Repositories.Teachers
{
    public interface ICourseTemplateRepository : IGenericRepository<CourseTemplate>
    {
        Task<CourseTemplate?> GetCourseTemplateWithTeacherCourseByCourseTemplateIdAsync(
            Guid courseTemplateId,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<(List<CourseTemplate>, int totalCount)> GetCourseTemplatesWithCourseByTeacherIdAsync(
            Guid teacherId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<CourseTemplate?> GetCourseTemplatesWithCourseByTeacherIdAsync(
            Guid courseTemplateId,
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
