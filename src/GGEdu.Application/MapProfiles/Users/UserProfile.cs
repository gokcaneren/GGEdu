using AutoMapper;
using GGEdu.Core.DTOs.Users.Outputs;
using GGEdu.Core.Entities.Users;

namespace GGEdu.Application.MapProfiles.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserProfileOutputDto>();
        }
    }
}
