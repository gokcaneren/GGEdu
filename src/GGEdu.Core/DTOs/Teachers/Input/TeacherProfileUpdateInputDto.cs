using System.ComponentModel.DataAnnotations;

namespace GGEdu.Core.DTOs.Teachers.Input
{
    public class TeacherProfileUpdateInputDto
    {
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public List<TeacherCourseInputDto> TeacherCourses { get; set; }
    }
}
