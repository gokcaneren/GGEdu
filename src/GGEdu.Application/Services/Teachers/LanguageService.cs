using AutoMapper;
using GGEdu.Core.DTOs.Languages.Input;
using GGEdu.Core.DTOs.Languages.Output;
using GGEdu.Core.Entities.Teachers.Languages;
using GGEdu.Core.Enums;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Core.Services.Teachers;
using GGEdu.Core.UnitOfWorks;
using GGEdu.Core.Utilities;
using GGEdu.Core.Utilities.Extensions;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace GGEdu.Application.Services.Teachers
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResources> _localizer;

        public LanguageService(
            ILanguageRepository languageRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<ApiResponse<bool>> AddLanguageAsync(
            LanguageInputDto languageInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var existEntity = await _languageRepository.GetByAsync(c => c.Code.Equals(languageInputDto.Code));

                if (existEntity != null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Lng.LanguageAlreadyExistError"], false);
                }

                var language = _mapper.Map<Language>(languageInputDto);

                await _languageRepository.CreateAsync(language, autoSave: false, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.Created, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> AddLanguagesAsync(
            List<LanguageInputDto> languageInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var languageList = await _languageRepository.GetAllAsync(cancellationToken);

                if (languageList.Any(c => languageInputDto.Select(x => x.Code).Contains(c.Code)))
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Lng.LanguageAlreadyExistError"], false);
                }

                var newLanguages = _mapper.Map<List<Language>>(languageInputDto);

                await _languageRepository.CreateRangeAsync(newLanguages, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.Created, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> DeleteLanguageByCodeAsync(
            string code,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var language = await _languageRepository.GetByAsync(c => c.Code.Equals(code), cancellationToken);

                if (language == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.Found, _localizer["Lng.LanguageNotFoundError"], false);
                }

                _languageRepository.Delete(language);
                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<LanguageOutputDto>> GetLanguageByCodeAsync(
            string code,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var language = await _languageRepository.GetByAsync(c => c.Code.Equals(code), cancellationToken);

                if (language == null)
                {
                    return ApiResponse<LanguageOutputDto>.ErrorResponse(
                        HttpStatusCode.Found, _localizer["Lng.LanguageNotFoundError"], null);
                }

                var languageOutputDto = _mapper.Map<LanguageOutputDto>(language);

                return ApiResponse<LanguageOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], languageOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ApiResponse<List<LanguageLevelOutputDto>> GetLanguageLevels()
        {
            try
            {
                var languageLevels = Enum.GetValues<LanguageLevel>()
                    .Select(x => new LanguageLevelOutputDto
                    {
                        Id = (int)x,
                        Name = x.GetDescription()
                    })
                    .ToList();

                return ApiResponse<List<LanguageLevelOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK,
                    _localizer["Gnrl.Success"],
                    languageLevels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<List<LanguageOutputDto>>> GetLanguagesAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var languages = await _languageRepository.GetAllAsync(cancellationToken);

                var languagesOutputDtos = _mapper.Map<List<LanguageOutputDto>>(languages);

                return ApiResponse<List<LanguageOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], languagesOutputDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
