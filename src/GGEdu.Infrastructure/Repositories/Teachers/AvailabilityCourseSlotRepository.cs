using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Enums;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Infrastructure.Context;
using GGEdu.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GGEdu.Infrastructure.Repositories.Teachers
{
    public class AvailabilityCourseSlotRepository : GenericRepository<AvailabilityCourseSlot>, IAvailabilityCourseSlotRepository
    {
        public AvailabilityCourseSlotRepository(GGEduContext ggEduContext) : base(ggEduContext)
        {
        }

        public async Task<List<AvailabilityCourseSlot>> GetCourseSlotsWithTeacherCourseAsync(
            Guid teacherId,
            DateTime? fromDate,
            DateTime? toDate,
            AvailabilityCourseSlotStatus availabilityCourseSlotStatus,
            CancellationToken cancellationToken = default)
        {
            fromDate = fromDate != null ? DateTime.SpecifyKind(fromDate.Value, DateTimeKind.Utc) : null;
            toDate = toDate != null ? DateTime.SpecifyKind(toDate.Value, DateTimeKind.Utc) : null;

            return await _ggEduContext.AvailabilityCourseSlots
                .WhereIf(fromDate == null, c=> c.StartAtUtc >= DateTime.UtcNow)
                .WhereIf(fromDate != null, c => c.StartAtUtc >= fromDate)
                .WhereIf(toDate != null, c => c.EndAtUtc <= toDate)
                .Where(c => c.TeacherId.Equals(teacherId) && c.Status == availabilityCourseSlotStatus)
                .Include(c => c.TeacherCourse)
                .ThenInclude(c => c.Course)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AvailabilityCourseSlot>> GetExistingSlotsAsync(
            Guid courseTemplateId,
            DateTime fromDate,
            DateTime toDate,
            CancellationToken cancellationToken = default)
        {
            fromDate = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);
            toDate = DateTime.SpecifyKind(toDate, DateTimeKind.Utc);

            return await _ggEduContext.AvailabilityCourseSlots
        .Where(c =>
            c.CourseTemplateId.Equals(courseTemplateId) &&
            c.StartAtUtc >= fromDate &&
            c.EndAtUtc <= toDate)
        .ToListAsync(cancellationToken);
        }

        public async Task<List<AvailabilityCourseSlot>> GetTeacherCourseSlotByTeacherIdAndCourseCodeAsync(
            Guid teacherId,
            string courseCode,
            CancellationToken cancellationToken = default)
        {
            var fromDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            var toDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc);

            return await _ggEduContext.AvailabilityCourseSlots
                .Where(c => c.TeacherId.Equals(teacherId) && c.Status == AvailabilityCourseSlotStatus.Available
                && c.StartAtUtc >= fromDate && c.EndAtUtc <= toDate
                && c.TeacherCourse.Course.Code.Equals(courseCode))
                .Include(c => c.TeacherCourse)
                .ThenInclude(c => c.Course)
                .ToListAsync(cancellationToken);
        }
    }
}
