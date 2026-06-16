namespace GGEdu.Core.DTOs.Users.Inputs
{
    public class UserRegisterInputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsTeacher { get; set; }
    }
}
