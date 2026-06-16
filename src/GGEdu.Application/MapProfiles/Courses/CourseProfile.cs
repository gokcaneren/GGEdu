using AutoMapper;
using GGEdu.Core.DTOs.Courses.Input;
using GGEdu.Core.DTOs.Courses.Output;
using GGEdu.Core.Entities.Teachers.Courses;

namespace GGEdu.Application.MapProfiles.Courses
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseInputDto, Course>()
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Code.Trim().ToLower()));

            CreateMap<Course, CourseOutputDto>();
        }
    }
}
