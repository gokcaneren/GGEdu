using GGEdu.Core.DTOs.Teachers.Input;
using GGEdu.Core.DTOs.Teachers.Output;
using GGEdu.Core.Utilities;

namespace GGEdu.Core.Services.Teachers
{
    public interface ITeacherService
    {
        Task<ApiResponse<TeacherDetailOutputDto>> UpdateTeacherProfileAsync(
            TeacherProfileUpdateInputDto teacherProfileUpdateInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<TeacherOutputDto>> GetTeacherProfileAsync(
            CancellationToken cancellationToken = default);

        Task<ApiResponse<TeacherDetailOutputDto>> GetTeacherDetailsAsync(
            CancellationToken cancellationToken = default);

        Task<ApiResponse<TeacherOutputDto>> GetTeacherProfileByTeacherIdAsync(
            Guid teacherId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<TeacherDetailOutputDto>> GetTeacherDetailsByTeacherIdAsync(
            Guid teacherId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> DeleteTeacherCourseByTeacherCourseId(
            Guid teacherCourseId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<List<TeacherLanguageOutputDto>>> UpdateTeacherLanguageAsync(
            List<TeacherLanguageInputDto> teacherLanguageInputDtos,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<List<TeacherLanguageOutputDto>>> GetTeacherLanguagesByUserIdAsync(
            CancellationToken cancellationToken = default);

        Task<ApiResponse<List<TeacherCourseOutputDto>>> GetTeacherCoursesAsync(
            CancellationToken cancellationToken = default);
    }
}
