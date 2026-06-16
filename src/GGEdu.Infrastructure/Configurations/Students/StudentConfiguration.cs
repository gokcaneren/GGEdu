using GGEdu.Core.Entities.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Students
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c=>c.DisplayName).IsRequired(false);
            builder.Property(c=>c.Bio).IsRequired(false);

            builder.HasOne(c=>c.User)
                .WithOne(c => c.Student)
                .HasForeignKey<Student>(c=>c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
