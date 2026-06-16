using GGEdu.Core.DTOs.Teachers.Output;

namespace GGEdu.Core.DTOs.Courses.Output
{
    public class CourseWithTeachersOutputDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<TeacherAllDetailsOutputDto> TeacherAllDetailsOutputDtos{ get; set; }
    }
}
