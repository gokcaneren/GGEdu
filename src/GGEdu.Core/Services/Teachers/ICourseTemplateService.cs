using GGEdu.Core.DTOs.Courses.AvailabilityCourseSlot.Input;
using GGEdu.Core.DTOs.Courses.CourseTemplates.Input;
using GGEdu.Core.DTOs.Courses.CourseTemplates.Output;
using GGEdu.Core.DTOs.Courses.TeacherCourses.Input;
using GGEdu.Core.DTOs.Courses.TeacherCourses.Output;
using GGEdu.Core.Utilities;

namespace GGEdu.Core.Services.Teachers
{
    public interface ICourseTemplateService
    {
        Task<ApiResponse<Guid>> CreateCourseTemplateAsync(
            CourseTemplateInputDto courseTemplateInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> GenerateSlotsFromCourseTemplateAsync(
            AvailabilityCourseSlotInputDto availabilityCourseSlotInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<CourseTemplateOutputDto>> GetCourseTemplateByIdAsync(
            Guid courseTemplateId,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<List<CourseTemplateOutputDto>>> GetCourseTemplatesAsync(
            CancellationToken cancellationToken = default);
        
        Task<ApiResponse<List<TeacherCourseWithCourseSlotOutputDto>>> GetCourseSlotsWithTeacherCourseAsync(
            TeacherCourseWithCourseSlotInputDto teacherCourseWithCourseSlotInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<PagedResponse<CourseTemplateSimpleOutputDto>>> GetCourseTemplatesWithCourseAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<CourseTemplateWithCourseOutputDto>> GetCourseTemplateWithCourseByCourseTemplateIdAsync(
            Guid courseTemplateId,
            CancellationToken cancellationToken = default);
    }
}
