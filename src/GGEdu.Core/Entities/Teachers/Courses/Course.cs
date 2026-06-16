namespace GGEdu.Core.Entities.Teachers.Courses
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<TeacherCourse> TeacherCourses { get; set; }
    }
}
