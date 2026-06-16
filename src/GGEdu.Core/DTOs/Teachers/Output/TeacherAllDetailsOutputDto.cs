using GGEdu.Core.DTOs.Users.Outputs;

namespace GGEdu.Core.DTOs.Teachers.Output
{
    public class TeacherAllDetailsOutputDto
    {
        public UserProfileOutputDto UserProfileDetail { get; set; }
        public TeacherOutputDto TeacherProfileDetail { get; set; } 
        public TeacherCourseProfileOutputDto TeacherCourseProfileDetail { get; set; } 
        public List<TeacherLanguageOutputDto> TeacherLanguageProfileDetails { get; set; } 
    }
}
