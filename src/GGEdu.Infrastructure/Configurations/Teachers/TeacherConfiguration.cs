using GGEdu.Core.Entities.Teachers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Teachers
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Bio).IsRequired(false);
            builder.Property(c => c.DisplayName).IsRequired(false);

            builder.Property(c => c.DurationMinutes).IsRequired();
            builder.Property(c => c.TimeZoneId).IsRequired();

            builder.HasOne(c => c.User)
                .WithOne(c => c.Teacher)
                .HasForeignKey<Teacher>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
