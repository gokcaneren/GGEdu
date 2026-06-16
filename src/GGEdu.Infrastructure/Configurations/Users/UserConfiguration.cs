using GGEdu.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName).IsRequired();
            builder.Property(c => c.LastName).IsRequired();
            builder.Property(c=>c.Photo).IsRequired(false);
            builder.Property(c => c.Gender).IsRequired();

            builder.Property(c => c.Email).IsRequired();
            builder.Property(c => c.NormalizedEmail).IsRequired();
            builder.Property(c => c.EmailConfirmed).IsRequired();

            builder.Property(c => c.PasswordHash).IsRequired();
            builder.Property(c => c.SecurityStamp).IsRequired(false);
        }
    }
}
