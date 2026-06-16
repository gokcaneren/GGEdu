using GGEdu.Core.DTOs.Courses.AvailabilityCourseSlot.Input;
using GGEdu.Core.DTOs.Courses.CourseTemplates.Input;
using GGEdu.Core.DTOs.Courses.CourseTemplates.Output;
using GGEdu.Core.DTOs.Courses.TeacherCourses.Input;
using GGEdu.Core.DTOs.Courses.TeacherCourses.Output;
using GGEdu.Core.Services.Teachers;
using GGEdu.Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GGEdu.API.Controllers.Courses
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseTemplateController : ControllerBase
    {
        private readonly ICourseTemplateService _courseTemplateService;

        public CourseTemplateController(ICourseTemplateService courseTemplateService)
        {
            _courseTemplateService = courseTemplateService;
        }

        [HttpPost]
        public async Task<ApiResponse<Guid>> CreateCourseTemplateAsync(
            CourseTemplateInputDto courseTemplateInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _courseTemplateService.CreateCourseTemplateAsync(courseTemplateInputDto, cancellationToken);
        }

        [HttpPost("generate-slots")]
        public async Task<ApiResponse<bool>> GenerateSlotsFromCourseTemplateAsync(
            AvailabilityCourseSlotInputDto availabilityCourseSlotInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _courseTemplateService.GenerateSlotsFromCourseTemplateAsync(availabilityCourseSlotInputDto, cancellationToken);
        }

        [HttpGet("slots")]
        public async Task<ApiResponse<List<TeacherCourseWithCourseSlotOutputDto>>> GetCourseSlotsWithTeacherCourseAsync(
            [FromQuery]TeacherCourseWithCourseSlotInputDto teacherCourseWithCourseSlotInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _courseTemplateService.GetCourseSlotsWithTeacherCourseAsync(teacherCourseWithCourseSlotInputDto, cancellationToken);
        }

        //[HttpGet("{courseTemplateId}")]
        //public async Task<ApiResponse<CourseTemplateOutputDto>> GetCourseTemplateByIdAsync(
        //    Guid courseTemplateId,
        //    CancellationToken cancellationToken = default)
        //{
        //    return await _courseTemplateService.GetCourseTemplateByIdAsync(courseTemplateId, cancellationToken);
        //}

        [HttpGet("{courseTemplateId}")]
        public async Task<ApiResponse<CourseTemplateWithCourseOutputDto>> GetCourseTemplateByIdAsync(
            Guid courseTemplateId,
            CancellationToken cancellationToken = default)
        {
            return await _courseTemplateService.GetCourseTemplateWithCourseByCourseTemplateIdAsync(courseTemplateId, cancellationToken);
        }


        //[HttpGet("all")]
        //public async Task<ApiResponse<List<CourseTemplateOutputDto>>> GetCourseTemplatesAsync(
        //    CancellationToken cancellationToken = default)
        //{
        //    return await _courseTemplateService.GetCourseTemplatesAsync(cancellationToken);
        //}

        [HttpGet("all")]
        public async Task<ApiResponse<PagedResponse<CourseTemplateSimpleOutputDto>>> GetCourseTemplatesAsync(
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _courseTemplateService.GetCourseTemplatesWithCourseAsync(page, pageSize, cancellationToken);
        }
    }
}
