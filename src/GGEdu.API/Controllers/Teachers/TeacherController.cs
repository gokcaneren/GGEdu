using GGEdu.Core.DTOs.Teachers.Input;
using GGEdu.Core.DTOs.Teachers.Output;
using GGEdu.Core.Services.Teachers;
using GGEdu.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GGEdu.API.Controllers.Teachers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("profile")]
        public async Task<ApiResponse<TeacherOutputDto>> GetProfileAsync(
            CancellationToken cancellationToken = default)
        {
            return await _teacherService.GetTeacherProfileAsync(cancellationToken);
        }

        [HttpGet("details")]
        public async Task<ApiResponse<TeacherDetailOutputDto>> GetDetailsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _teacherService.GetTeacherDetailsAsync(cancellationToken);
        }

        [HttpGet("{teacherId}/profile")]
        public async Task<ApiResponse<TeacherOutputDto>> GetProfileByTeacherIdAsync(
            Guid teacherId,
            CancellationToken cancellationToken = default)
        {
            return await _teacherService.GetTeacherProfileByTeacherIdAsync(teacherId, cancellationToken);
        }

        [HttpGet("{teacherId}/details")]
        public async Task<ApiResponse<TeacherDetailOutputDto>> GetDetailsByTeacherIdAsync(
            Guid teacherId,
            CancellationToken cancellationToken = default)
        {
            return await _teacherService.GetTeacherDetailsByTeacherIdAsync(teacherId, cancellationToken);
        }

        [HttpPut]
        public async Task<ApiResponse<TeacherDetailOutputDto>> UpdateTeacherProfileAsync(
            TeacherProfileUpdateInputDto teacherProfileUpdateInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _teacherService.UpdateTeacherProfileAsync(teacherProfileUpdateInputDto, cancellationToken);
        }

        [HttpDelete("{teacherCourseId}/teacher-course")]
        public async Task<ApiResponse<bool>> DeleteTeacherCourseAsync(
            Guid teacherCourseId,
            CancellationToken cancellationToken = default)
        {
            return await _teacherService.DeleteTeacherCourseByTeacherCourseId(teacherCourseId, cancellationToken);
        }

        [HttpGet("languages")]
        public async Task<ApiResponse<List<TeacherLanguageOutputDto>>> GetTeacherLanguagesByTeacherIdAsync(
            CancellationToken cancellationToken = default)
        {
            return await _teacherService.GetTeacherLanguagesByUserIdAsync(cancellationToken);
        }

        [HttpPut("languages")]
        public async Task<ApiResponse<List<TeacherLanguageOutputDto>>> UpdateTeacherLanguageAsync(
            List<TeacherLanguageInputDto> teacherLanguageInputDtos,
            CancellationToken cancellationToken = default)
        {
            return await _teacherService.UpdateTeacherLanguageAsync(teacherLanguageInputDtos, cancellationToken);
        }

        [HttpGet("courses")]
        public async Task<ApiResponse<List<TeacherCourseOutputDto>>> GetTeacherCoursesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _teacherService.GetTeacherCoursesAsync(cancellationToken);
        }
    }
}
