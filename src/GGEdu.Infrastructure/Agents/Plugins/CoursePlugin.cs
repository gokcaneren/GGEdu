using GGEdu.Core.Repositories.Teachers;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace GGEdu.Infrastructure.Agents.Plugins
{
    public class CoursePlugin
    {
        private readonly ITeacherCourseRepository _teacherCourseRepository;

        public CoursePlugin(
            ITeacherCourseRepository teacherCourseRepository)
        {
            _teacherCourseRepository = teacherCourseRepository;
        }


        [KernelFunction("get_course")]
        [Description("It brings course.")]
        public async Task<string> GetCourseAsync(
            [Description("Teacher Id")] Guid teacherId,
            [Description("Course code, examp: tr for Turkish")] string courseCode)
        {
            var teacherCourse = await _teacherCourseRepository.GetTeacherCourseWithCourseAsync(teacherId, courseCode);

            if(teacherCourse == null)
            {
                return null;
            }

            return string.Join(",", teacherCourse.Course.Name);
        }
    }
}
