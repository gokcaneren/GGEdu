using AutoMapper;
using GGEdu.Core.DTOs.Courses.CourseTemplates.Input;
using GGEdu.Core.DTOs.Courses.CourseTemplates.Output;
using GGEdu.Core.Entities.Teachers.Courses;

namespace GGEdu.Application.MapProfiles.Courses
{
    public class CourseTemplateProfile : Profile
    {
        public CourseTemplateProfile()
        {
            CreateMap<CourseTemplateInputDto, CourseTemplate>();
            CreateMap<CourseTemplate, CourseTemplateOutputDto>();

            CreateMap<CourseTemplate, CourseTemplateWithCourseOutputDto>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.TeacherCourse.CourseId))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.TeacherCourse.Course.Code));

            CreateMap<CourseTemplate, CourseTemplateSimpleOutputDto>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.TeacherCourse.CourseId))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.TeacherCourse.Course.Code));
        }
    }
}
