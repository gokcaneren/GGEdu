using GGEdu.Core.DTOs.Languages.Input;
using GGEdu.Core.DTOs.Languages.Output;
using GGEdu.Core.Services.Teachers;
using GGEdu.Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GGEdu.API.Controllers.Languages
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> AddLanguageAsync(
            LanguageInputDto languageInputDto,
            CancellationToken cancellationToken = default)
        {
            return await _languageService.AddLanguageAsync(languageInputDto, cancellationToken);
        }

        [HttpPost("languages")]
        public async Task<ApiResponse<bool>> AddLanguagesAsync(
            List<LanguageInputDto> languageInputDtos,
            CancellationToken cancellationToken = default)
        {
            return await _languageService.AddLanguagesAsync(languageInputDtos, cancellationToken);
        }

        [HttpGet("{code}/language")]
        public async Task<ApiResponse<LanguageOutputDto>> GetLanguageByCodeAsync(
            string code,
            CancellationToken cancellationToken = default)
        {
            return await _languageService.GetLanguageByCodeAsync(code, cancellationToken);
        }

        [HttpDelete("{code}/language")]
        public async Task<ApiResponse<bool>> DeleteLanguageByCodeAsync(
            string code,
            CancellationToken cancellationToken = default)
        {
            return await _languageService.DeleteLanguageByCodeAsync(code, cancellationToken);
        }

        [HttpGet("all")]
        public async Task<ApiResponse<List<LanguageOutputDto>>> GetLanguagesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _languageService.GetLanguagesAsync(cancellationToken);
        }

        [HttpGet("levels")]
        public ApiResponse<List<LanguageLevelOutputDto>> GetLanguageLevels()
        {
            return _languageService.GetLanguageLevels();
        }
    }
}
