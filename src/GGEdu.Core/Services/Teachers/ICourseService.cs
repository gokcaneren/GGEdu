using GGEdu.Core.DTOs.Courses.Input;
using GGEdu.Core.DTOs.Courses.Output;
using GGEdu.Core.Utilities;

namespace GGEdu.Core.Services.Teachers
{
    public interface ICourseService
    {
        Task<ApiResponse<bool>> CreateCoursesAsync(
            List<CourseInputDto> courseInputDtos,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> CreateCourseAsync(
            CourseInputDto courseInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<List<CourseOutputDto>>> GetCoursesAsync(
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> DeleteCourseAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<CourseWithTeachersOutputDto>> GetCourseWithTeachersByCourseCodeAsync(
            string courseCode,
            CancellationToken cancellationToken = default);
    }
}
