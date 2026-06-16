using GGEdu.Core.DTOs.Courses.Input;
using GGEdu.Core.DTOs.Courses.Output;
using GGEdu.Core.Services.Teachers;
using GGEdu.Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GGEdu.API.Controllers.Courses
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("all")]
        public Task<ApiResponse<List<CourseOutputDto>>> GetCoursesAsync(
            CancellationToken cancellationToken = default)
        {
            return _courseService.GetCoursesAsync(cancellationToken);
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> CreateCourseAsync(
            CourseInputDto courseInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _courseService.CreateCourseAsync(courseInputDto, cancellationToken);
        }

        [HttpPost("list")]
        public async Task<ApiResponse<bool>> CreateCoursesAsync(
            List<CourseInputDto> courseInputDtos,
            CancellationToken cancellationToken = default)
        {
            return await _courseService.CreateCoursesAsync(courseInputDtos, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<bool>> DeleteCourseAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _courseService.DeleteCourseAsync(id, cancellationToken);
        }

        [HttpGet("{courseCode}/details")]
        public async Task<ApiResponse<CourseWithTeachersOutputDto>> GetCourseDetailsAsync(
            string courseCode,
            CancellationToken cancellationToken = default)
        {
            return await _courseService.GetCourseWithTeachersByCourseCodeAsync(courseCode, cancellationToken);
        }
    }
}
