using GGEdu.Core.DTOs.Languages.Input;
using GGEdu.Core.DTOs.Languages.Output;
using GGEdu.Core.Utilities;

namespace GGEdu.Core.Services.Teachers
{
    public interface ILanguageService
    {
        Task<ApiResponse<bool>> AddLanguageAsync(
            LanguageInputDto languageInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> AddLanguagesAsync(
            List<LanguageInputDto> languageInputDto,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<bool>> DeleteLanguageByCodeAsync(
            string code,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<List<LanguageOutputDto>>> GetLanguagesAsync(
            CancellationToken cancellationToken = default);

        Task<ApiResponse<LanguageOutputDto>> GetLanguageByCodeAsync(
            string code,
            CancellationToken cancellationToken = default);

        ApiResponse<List<LanguageLevelOutputDto>> GetLanguageLevels();
    }
}
