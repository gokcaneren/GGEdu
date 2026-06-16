using GGEdu.Core.Entities.Teachers.Languages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Teachers
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code).IsRequired();
        }
    }
}
