using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Core.Utilities;
using GGEdu.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GGEdu.Infrastructure.Repositories.Teachers
{
    public class CourseTemplateRepository : GenericRepository<CourseTemplate>, ICourseTemplateRepository
    {
        public CourseTemplateRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }

        public async Task<(List<CourseTemplate>, int totalCount)> GetCourseTemplatesWithCourseByTeacherIdAsync(
            Guid teacherId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _ggEduContext.CourseTemplates
                .Where(c => c.TeacherId.Equals(teacherId))
                .Include(c => c.TeacherCourse)
                .ThenInclude(c => c.Course)
                .OrderByDescending(c => c.CreatedDate);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<CourseTemplate?> GetCourseTemplatesWithCourseByTeacherIdAsync(
            Guid courseTemplateId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.CourseTemplates
                .AsQueryable()
                .Include(c => c.TeacherCourse)
                .ThenInclude(c=>c.Course)
                .FirstOrDefaultAsync(c =>
                c.Id.Equals(courseTemplateId) && c.Teacher.UserId.Equals(userId), cancellationToken);
        }

        public async Task<CourseTemplate?> GetCourseTemplateWithTeacherCourseByCourseTemplateIdAsync(
            Guid courseTemplateId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.CourseTemplates
                .AsQueryable()
                .Include(c => c.Teacher)
                .Include(c => c.TeacherCourse)
                .FirstOrDefaultAsync(c =>
                c.Id.Equals(courseTemplateId) && c.Teacher.UserId.Equals(userId), cancellationToken);
        }
    }
}
