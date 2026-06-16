using AutoMapper;
using GGEdu.Core.DTOs.Students.Input;
using GGEdu.Core.DTOs.Students.Output;
using GGEdu.Core.Entities.Students;

namespace GGEdu.Application.MapProfiles.Students
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentOutputDto>();
        }
    }
}
