using GGEdu.Core.Entities.Teachers.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Teachers
{
    public class TeacherCourseConfiguration : IEntityTypeConfiguration<TeacherCourse>
    {
        public void Configure(EntityTypeBuilder<TeacherCourse> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Price).IsRequired();
            builder.Property(c => c.Currency).IsRequired();
            builder.Property(c => c.DurationMinutes).IsRequired();


            builder.HasOne(c => c.Teacher)
            .WithMany(c => c.TeacherCourses)
            .HasForeignKey(c => c.TeacherId);

            builder.HasOne(c => c.Course)
            .WithMany(c => c.TeacherCourses)
            .HasForeignKey(c => c.CourseId);
        }
    }
}
