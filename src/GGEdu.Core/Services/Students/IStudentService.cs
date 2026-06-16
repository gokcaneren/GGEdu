using GGEdu.Core.DTOs.Students.Input;
using GGEdu.Core.DTOs.Students.Output;
using GGEdu.Core.Utilities;

namespace GGEdu.Core.Services.Students
{
    public interface IStudentService
    {
        Task<ApiResponse<StudentOutputDto>> GetProfileAsync(
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> UpdateProfileAsync(
            StudentProfileUpdateInputDto studentProfileInputDto,
            CancellationToken cancellationToken = default);
    }
}
