using GGEdu.Core.DTOs.Students.Input;
using GGEdu.Core.DTOs.Students.Output;
using GGEdu.Core.Services.Students;
using GGEdu.Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GGEdu.API.Controllers.Students
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("profile")]
        public async Task<ApiResponse<StudentOutputDto>> GetProfileAsync(
            CancellationToken cancellationToken = default)
        {
            return await _studentService.GetProfileAsync(cancellationToken);
        }

        [HttpPut("profile")]
        public async Task<ApiResponse<bool>> UpdateProfileAsync(
            StudentProfileUpdateInputDto studentProfileUpdateInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _studentService.UpdateProfileAsync(studentProfileUpdateInputDto, cancellationToken);
        }
    }
}
