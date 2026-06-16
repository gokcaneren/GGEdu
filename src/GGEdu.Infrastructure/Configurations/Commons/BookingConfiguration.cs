using GGEdu.Core.Entities.Commons.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Commons
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Status).IsRequired();

            builder.Property(c => c.DecisionDate).IsRequired(false);
            builder.Property(c => c.CancelledDate).IsRequired(false);
            builder.Property(c => c.CompletedDate).IsRequired(false);

            builder.HasOne(c => c.Teacher)
                .WithMany(c => c.Bookings)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Student)
                .WithMany(c => c.Bookings)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.AvailabilityCourseSlot)
                .WithMany(c => c.Bookings)
                .HasForeignKey(c => c.AvailabilityCourseSlotId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
