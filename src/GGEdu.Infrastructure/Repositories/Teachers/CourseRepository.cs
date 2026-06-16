using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GGEdu.Infrastructure.Repositories.Teachers
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }

        public async Task<Course?> GetCourseWithTeachersByCourseCodeAsync(
            string courseCode,
            CancellationToken cancellationToken = default)
        {
            return await _ggEduContext.Courses
                .AsSplitQuery()
                .Where(c => c.Code.Equals(courseCode))
                .Include(c => c.TeacherCourses)
                .ThenInclude(c => c.Teacher)
                .ThenInclude(c => c.TeacherLanguages)
                .ThenInclude(c => c.Language)

                .Include(x => x.TeacherCourses)
                .ThenInclude(x => x.Teacher)
                .ThenInclude(x => x.User)

                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
