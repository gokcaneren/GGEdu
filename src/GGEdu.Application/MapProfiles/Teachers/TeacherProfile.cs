using AutoMapper;
using GGEdu.Core.DTOs.Teachers.Output;
using GGEdu.Core.Entities.Teachers;

namespace GGEdu.Application.MapProfiles.Teachers
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherOutputDto>();
            CreateMap<Teacher, TeacherDetailOutputDto>();
        }
    }
}
