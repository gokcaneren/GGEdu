using GGEdu.Core.Enums;

namespace GGEdu.Core.DTOs.Users.Outputs
{
    public class UserSignInOutputDto
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRoles Role { get; set; }
    }
}
