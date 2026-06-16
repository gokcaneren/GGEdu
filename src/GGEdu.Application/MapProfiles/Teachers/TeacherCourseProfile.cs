using AutoMapper;
using GGEdu.Core.DTOs.Teachers.Output;
using GGEdu.Core.Entities.Teachers.Courses;

namespace GGEdu.Application.MapProfiles.Teachers
{
    public class TeacherCourseProfile : Profile
    {
        public TeacherCourseProfile()
        {
            CreateMap<TeacherCourse, TeacherCourseOutputDto>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Course.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Course.Code));

            CreateMap<TeacherCourse, TeacherCourseProfileOutputDto>();
        }
    }
}
