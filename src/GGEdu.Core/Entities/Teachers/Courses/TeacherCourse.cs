using GGEdu.Core.Enums;

namespace GGEdu.Core.Entities.Teachers.Courses
{
    public class TeacherCourse : BaseEntity
    {
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public Currencies Currency { get; set; } = Currencies.USD;

        public Guid TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
