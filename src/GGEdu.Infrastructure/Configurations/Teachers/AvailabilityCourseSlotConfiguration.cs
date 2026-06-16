using GGEdu.Core.Entities.Teachers.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Teachers
{
    public class AvailabilityCourseSlotConfiguration : IEntityTypeConfiguration<AvailabilityCourseSlot>
    {
        public void Configure(EntityTypeBuilder<AvailabilityCourseSlot> builder)
        {
            builder.HasKey(c=>c.Id);

            builder.Property(c=>c.StartAtUtc).IsRequired();
            builder.Property(c=>c.EndAtUtc).IsRequired();

            builder.Property(c=>c.Status).IsRequired();

            builder.HasOne(c=>c.Teacher)
                .WithMany(c=>c.AvailabilityCourseSlots)
                .HasForeignKey(c=>c.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c=>c.CourseTemplate)
                .WithMany()
                .HasForeignKey(c=>c.CourseTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.TeacherCourse)
                .WithMany()
                .HasForeignKey(c => c.TeacherCourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
