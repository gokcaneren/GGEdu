using GGEdu.Core.Entities.Teachers.Languages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Teachers
{
    public class TeacherLanguageConfiguration : IEntityTypeConfiguration<TeacherLanguage>
    {
        public void Configure(EntityTypeBuilder<TeacherLanguage> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Level).IsRequired();

            builder.HasOne(c=>c.Teacher)
                .WithMany(c=>c.TeacherLanguages)
                .HasForeignKey(c=>c.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c=>c.Language)
                .WithOne()
                .HasForeignKey<TeacherLanguage>(c=>c.LanguageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
