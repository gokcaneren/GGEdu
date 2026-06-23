using GGEdu.Core.Entities.Teachers.Courses;

namespace GGEdu.Core.Repositories.Teachers
{
    public interface ITeacherCourseRepository : IGenericRepository<TeacherCourse>
    {
        Task<TeacherCourse?> GetTeacherCourseWithCourseAsync(
            Guid teacherId,
            string courseCode);
    }
}
