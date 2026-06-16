using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Infrastructure.Context;

namespace GGEdu.Infrastructure.Repositories.Teachers
{
    public class TeacherCourseRepository : GenericRepository<TeacherCourse>, ITeacherCourseRepository
    {
        public TeacherCourseRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }
    }
}
