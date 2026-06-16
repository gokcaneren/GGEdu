using GGEdu.Core.Entities.Teachers.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Teachers
{
    public class CourseTemplateConfiguration : IEntityTypeConfiguration<CourseTemplate>
    {
        public void Configure(EntityTypeBuilder<CourseTemplate> builder)
        {
            builder.HasKey(c=>c.Id);

            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.DayOfWeek).IsRequired();

            builder.Property(c => c.StartLocalTime).IsRequired();
            builder.Property(c => c.EndLocalTime).IsRequired();

            builder.Property(c => c.TimeZoneId).IsRequired().HasDefaultValue("UTC");

            builder.Property(c => c.EffectiveFrom).IsRequired(false);
            builder.Property(c => c.EffectiveTo).IsRequired(false);

            builder.Property(c => c.AutoGenerateSlots).IsRequired();
            builder.Property(c => c.GenerateDaysAhead).IsRequired();

            builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);

            builder.HasOne(c=>c.Teacher)
                .WithMany(c=>c.CourseTemplates)
                .HasForeignKey(c=>c.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.TeacherCourse)
                .WithMany()
                .HasForeignKey(c => c.TeacherCourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
