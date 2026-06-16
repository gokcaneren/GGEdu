using GGEdu.Core.Entities.Teachers;
using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Enums;

namespace GGEdu.Core.Repositories.Teachers
{
    public interface IAvailabilityCourseSlotRepository : IGenericRepository<AvailabilityCourseSlot>
    {
        Task<List<AvailabilityCourseSlot>> GetExistingSlotsAsync(
                Guid courseTemplateId,
                DateTime fromUtc,
                DateTime toUtc,
                CancellationToken cancellationToken = default);

        Task<List<AvailabilityCourseSlot>> GetCourseSlotsWithTeacherCourseAsync(
            Guid teacherId,
            DateTime? fromDate,
            DateTime? toDate,
            AvailabilityCourseSlotStatus availabilityCourseSlotStatus,
            CancellationToken cancellationToken = default);

        Task<List<AvailabilityCourseSlot>> GetTeacherCourseSlotByTeacherIdAndCourseCodeAsync(
            Guid teacherId,
            string courseCode,
            CancellationToken cancellationToken = default);
    }
}
