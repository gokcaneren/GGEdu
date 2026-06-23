using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GGEdu.Infrastructure.Repositories.Teachers
{
    public class TeacherCourseRepository : GenericRepository<TeacherCourse>, ITeacherCourseRepository
    {
        public TeacherCourseRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }

        public async Task<TeacherCourse?> GetTeacherCourseWithCourseAsync(Guid teacherId, string courseCode)
        {
            return await _ggEduContext.TeacherCourses
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.TeacherId.Equals(teacherId) && c.Course.Code.Equals(courseCode));
        }
    }
}
