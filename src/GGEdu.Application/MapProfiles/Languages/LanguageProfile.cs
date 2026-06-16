using AutoMapper;
using GGEdu.Core.DTOs.Languages.Input;
using GGEdu.Core.DTOs.Languages.Output;
using GGEdu.Core.Entities.Teachers.Languages;

namespace GGEdu.Application.MapProfiles.Languages
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<LanguageInputDto, Language>();
            CreateMap<Language, LanguageOutputDto>();
        }
    }
}
