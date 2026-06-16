using GGEdu.Core.Entities.Commons.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GGEdu.Infrastructure.Configurations.Commons
{
    public class BookingRequestConfiguration : IEntityTypeConfiguration<BookingRequest>
    {
        public void Configure(EntityTypeBuilder<BookingRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Status).IsRequired();
            builder.Property(c => c.DecisionDate).IsRequired(false);

            builder.HasOne(c=>c.Teacher)
                .WithMany(c=>c.BookingRequests)
                .HasForeignKey(c=>c.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Student)
                .WithMany(c => c.BookingRequests)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.AvailabilityCourseSlot)
                .WithMany()
                .HasForeignKey(c => c.AvailabilityCourseSlotId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
