using GGEdu.Core.Entities.Commons.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Commons
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c=>c.Status).IsRequired();
            builder.Property(c=>c.DecisionDate).IsRequired(false);

            builder.HasOne(c=>c.Teacher)
                .WithMany(c=>c.Subscribers)
                .HasForeignKey(c=>c.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Student)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
