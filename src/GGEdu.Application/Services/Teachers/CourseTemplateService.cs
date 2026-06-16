using AutoMapper;
using GGEdu.Core.DTOs.Courses.AvailabilityCourseSlot.Input;
using GGEdu.Core.DTOs.Courses.AvailabilityCourseSlot.Output;
using GGEdu.Core.DTOs.Courses.CourseTemplates.Input;
using GGEdu.Core.DTOs.Courses.CourseTemplates.Output;
using GGEdu.Core.DTOs.Courses.TeacherCourses.Input;
using GGEdu.Core.DTOs.Courses.TeacherCourses.Output;
using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Enums;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Core.Services.Teachers;
using GGEdu.Core.Services.Users;
using GGEdu.Core.UnitOfWorks;
using GGEdu.Core.Utilities;
using GGEdu.Infrastructure.Extensions;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace GGEdu.Application.Services.Teachers
{
    public class CourseTemplateService : ICourseTemplateService
    {
        private readonly ICourseTemplateRepository _courseTemplateRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IAvailabilityCourseSlotRepository _availabilityCourseSlotRepository;

        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResources> _localizer;

        public CourseTemplateService(
            ICourseTemplateRepository courseTemplateRepository,
            ITeacherRepository teacherRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer,
            IMapper mapper,
            IAvailabilityCourseSlotRepository availabilityCourseSlotRepository)
        {
            _courseTemplateRepository = courseTemplateRepository;
            _teacherRepository = teacherRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _mapper = mapper;
            _availabilityCourseSlotRepository = availabilityCourseSlotRepository;
        }

        public async Task<ApiResponse<Guid>> CreateCourseTemplateAsync(
            CourseTemplateInputDto courseTemplateInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetTeacherWithTeacherCoursesByUserIdAsync(userId, cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<Guid>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.TeacherCantBeFound"], Guid.Empty);
                }

                var teacherCourse = teacher.TeacherCourses.FirstOrDefault(c => c.Id.Equals(courseTemplateInputDto.TeacherCourseId));

                if (teacherCourse == null)
                {
                    return ApiResponse<Guid>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcrcrs.TeacherCourseNotFoundError"], Guid.Empty);
                }

                if (courseTemplateInputDto.StartLocalTime >= courseTemplateInputDto.EndLocalTime)
                {
                    return ApiResponse<Guid>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Ct.InvalidTimeRangeError"], Guid.Empty);
                }

                if (courseTemplateInputDto.EffectiveFrom.HasValue && courseTemplateInputDto.EffectiveTo.HasValue &&
                    courseTemplateInputDto.EffectiveFrom > courseTemplateInputDto.EffectiveTo)
                {
                    return ApiResponse<Guid>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Ct.InvalidDateRangeError"], Guid.Empty);
                }

                var isTimeZoneCorrect =
                    TimeZoneInfo.TryFindSystemTimeZoneById(courseTemplateInputDto.TimeZoneId, out var timeZone);

                if (!isTimeZoneCorrect)
                {
                    return ApiResponse<Guid>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Ct.InvalidTimeZoneError"], Guid.Empty);
                }

                var hasOverlap = await _courseTemplateRepository.AnyAsync(c =>
                c.TeacherId.Equals(teacher.Id) &&
                c.TeacherCourseId.Equals(courseTemplateInputDto.TeacherCourseId) &&
                c.TimeZoneId == courseTemplateInputDto.TimeZoneId &&
                c.DayOfWeek == courseTemplateInputDto.DayOfWeek &&
                courseTemplateInputDto.StartLocalTime < c.EndLocalTime &&
                courseTemplateInputDto.EndLocalTime > c.StartLocalTime,
                cancellationToken);

                if (hasOverlap)
                {
                    return ApiResponse<Guid>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Ct.TemplateOverlapError"], Guid.Empty);
                }

                var newCourseTemplate = _mapper.Map<CourseTemplate>(courseTemplateInputDto);
                newCourseTemplate.Id = Guid.NewGuid();
                newCourseTemplate.TeacherId = teacher.Id;

                await _courseTemplateRepository.CreateAsync(newCourseTemplate, autoSave: false, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<Guid>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], newCourseTemplate.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> GenerateSlotsFromCourseTemplateAsync(
            AvailabilityCourseSlotInputDto availabilityCourseSlotInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // TODO: İleride bu method üzerinde iyileştirmeler yapılmalı!! Slot oluştururken ders dakikası uygunluğuna dikkat edilmeli.
                var userId = _currentUserService.UserId;

                var courseTemplate = await _courseTemplateRepository.GetCourseTemplateWithTeacherCourseByCourseTemplateIdAsync(
                    availabilityCourseSlotInputDto.CourseTemplateId, userId, cancellationToken);

                if (courseTemplate == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Ct.TemplateNotFoundError"], false);
                }

                if (availabilityCourseSlotInputDto.FromDate > availabilityCourseSlotInputDto.ToDate)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Ct.InvalidDateRangeError"], false);
                }

                CheckEffectiveDates(availabilityCourseSlotInputDto, courseTemplate);

                var courseDurationMinutes = courseTemplate.TeacherCourse.DurationMinutes;
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(courseTemplate.TimeZoneId);

                var slotsToCreate = new List<AvailabilityCourseSlot>();

                var existingSlots = await _availabilityCourseSlotRepository.GetExistingSlotsAsync(
                    courseTemplate.Id,
                    availabilityCourseSlotInputDto.FromDate.ToDateTime(TimeOnly.MinValue),
                    availabilityCourseSlotInputDto.ToDate.ToDateTime(TimeOnly.MaxValue), cancellationToken);

                for (var currentDate = availabilityCourseSlotInputDto.FromDate;
                    currentDate <= availabilityCourseSlotInputDto.ToDate;
                    currentDate = currentDate.AddDays(1))
                {
                    if (currentDate.DayOfWeek != courseTemplate.DayOfWeek)
                    {
                        continue;
                    }

                    // Local datetime
                    var localStart = currentDate.ToDateTime(TimeOnly.FromTimeSpan(courseTemplate.StartLocalTime));

                    var localEnd = currentDate.ToDateTime(TimeOnly.FromTimeSpan(courseTemplate.EndLocalTime));

                    var currentSlotStart = localStart;

                    while (currentSlotStart.AddMinutes(courseDurationMinutes) <= localEnd)
                    {
                        var currentSlotEnd = currentSlotStart.AddMinutes(courseDurationMinutes);

                        // UTC Convert
                        var utcStart = TimeZoneInfo.ConvertTimeToUtc(currentSlotStart, timeZone);

                        var utcEnd = TimeZoneInfo.ConvertTimeToUtc(currentSlotEnd, timeZone);

                        var hasOverlap = existingSlots.Any(c =>
    utcStart < c.EndAtUtc &&
    utcEnd > c.StartAtUtc);
                        var hasOverlapInMemory = slotsToCreate.Any(c =>
    utcStart < c.EndAtUtc &&
    utcEnd > c.StartAtUtc);

                        // Duplicate Skip
                        if (!hasOverlap && !hasOverlapInMemory)
                        {
                            slotsToCreate.Add(new AvailabilityCourseSlot
                            {
                                TeacherId = courseTemplate.TeacherId,
                                TeacherCourseId = courseTemplate.TeacherCourseId,
                                CourseTemplateId = courseTemplate.Id,
                                StartAtUtc = utcStart,
                                EndAtUtc = utcEnd,
                                Status = AvailabilityCourseSlotStatus.Available
                            });
                        }

                        currentSlotStart = currentSlotEnd;
                    }
                }

                if (!slotsToCreate.IsNullOrEmpty())
                {
                    await _availabilityCourseSlotRepository.CreateRangeAsync(slotsToCreate, cancellationToken);
                    await _unitOfWork.CommitAsync(cancellationToken);
                }

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CheckEffectiveDates(AvailabilityCourseSlotInputDto availabilityCourseSlotInputDto, CourseTemplate courseTemplate)
        {
            if (courseTemplate.EffectiveFrom.HasValue)
            {
                availabilityCourseSlotInputDto.FromDate = courseTemplate.EffectiveFrom.Value;
            }

            if (courseTemplate.EffectiveTo.HasValue)
            {
                availabilityCourseSlotInputDto.ToDate = courseTemplate.EffectiveTo.Value;
            }
        }

        public async Task<ApiResponse<List<TeacherCourseWithCourseSlotOutputDto>>> GetCourseSlotsWithTeacherCourseAsync(
            TeacherCourseWithCourseSlotInputDto teacherCourseWithCourseSlotInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetByAsync(c=> c.UserId.Equals(userId), cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<List<TeacherCourseWithCourseSlotOutputDto>>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.TeacherCantBeFound"], null);
                }

                var courseSlots = await _availabilityCourseSlotRepository.GetCourseSlotsWithTeacherCourseAsync(
                    teacher.Id, teacherCourseWithCourseSlotInputDto.FromDate, teacherCourseWithCourseSlotInputDto.ToDate,
                    teacherCourseWithCourseSlotInputDto.SlotStatus, cancellationToken);

                var teacherCourseWithCourseSlotOutputDtos = courseSlots.GroupBy(c=> c.TeacherCourseId).Select(c=>
                {
                    var firstEntity = c.First();

                    return new TeacherCourseWithCourseSlotOutputDto
                    {
                        TeacherCourseId = firstEntity.TeacherCourseId,
                        CourseId = firstEntity.TeacherCourse.CourseId,
                        CourseCode = firstEntity.TeacherCourse.Course.Code,
                        CourseName = firstEntity.TeacherCourse.Course.Name,
                        Currency = firstEntity.TeacherCourse.Currency,
                        Price = firstEntity.TeacherCourse.Price,
                        DurationMinutes = firstEntity.TeacherCourse.DurationMinutes,
                        CourseSlots = c.OrderBy(x => x.StartAtUtc).Select(slot => new AvailabilityCourseSlotOutputDto
                        {
                            CourseTemplateId = slot.CourseTemplateId,
                            Id = slot.Id,
                            StartAtUtc = slot.StartAtUtc,
                            EndAtUtc = slot.EndAtUtc,
                            Status = slot.Status
                        }).ToList()
                    };
                }).ToList();

                return ApiResponse<List<TeacherCourseWithCourseSlotOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], teacherCourseWithCourseSlotOutputDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<CourseTemplateOutputDto>> GetCourseTemplateByIdAsync(
            Guid courseTemplateId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<CourseTemplateOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.TeacherCantBeFound"], null);
                }

                var courseTemplate = await _courseTemplateRepository.GetByAsync(c=>
                c.TeacherId.Equals(teacher.Id) && c.Id.Equals(courseTemplateId), cancellationToken);

                if (courseTemplate == null)
                {
                    return ApiResponse<CourseTemplateOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Ct.TemplateNotFoundError"], null);
                }

                var courseTemplateOutputDto = _mapper.Map<CourseTemplateOutputDto>(courseTemplate);

                return ApiResponse<CourseTemplateOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], courseTemplateOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<List<CourseTemplateOutputDto>>> GetCourseTemplatesAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<List<CourseTemplateOutputDto>>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.TeacherCantBeFound"], null);
                }

                var courseTemplates = await _courseTemplateRepository.GetListByAsync(c =>
                c.TeacherId.Equals(teacher.Id) && c.IsActive == true, cancellationToken);

                var courseTemplateOutputDtos = _mapper.Map<List<CourseTemplateOutputDto>>(courseTemplates);

                return ApiResponse<List<CourseTemplateOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], courseTemplateOutputDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<PagedResponse<CourseTemplateSimpleOutputDto>>> GetCourseTemplatesWithCourseAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<PagedResponse<CourseTemplateSimpleOutputDto>>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.TeacherCantBeFound"], null);
                }

                var (courseTemplates, totalCount) = await _courseTemplateRepository.GetCourseTemplatesWithCourseByTeacherIdAsync(
                    teacher.Id, page, pageSize, cancellationToken);


                if (courseTemplates.IsNullOrEmpty())
                {
                    return ApiResponse<PagedResponse<CourseTemplateSimpleOutputDto>>.ErrorResponse(
                        HttpStatusCode.NotFound, _localizer["Ct.TemplateNotFoundError"], null);
                }

                var courseTemplateWihtCourseOutputDtos = _mapper.Map<List<CourseTemplateSimpleOutputDto>>(courseTemplates);

                var pagedResponse = PagedResponse<CourseTemplateSimpleOutputDto>.CreatePagedResponse(
                    courseTemplateWihtCourseOutputDtos, totalCount, page, pageSize);

                return ApiResponse<PagedResponse<CourseTemplateSimpleOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], pagedResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<CourseTemplateWithCourseOutputDto>> GetCourseTemplateWithCourseByCourseTemplateIdAsync(
            Guid courseTemplateId, CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<CourseTemplateWithCourseOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.TeacherCantBeFound"], null);
                }

                var courseTemplate = await _courseTemplateRepository.GetCourseTemplateWithTeacherCourseByCourseTemplateIdAsync(
                    courseTemplateId, userId, cancellationToken);

                if(courseTemplate == null)
                {
                    return ApiResponse<CourseTemplateWithCourseOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Ct.TemplateNotFoundError"], null);
                }

                var courseTemplateWithCourseOutputDto = _mapper.Map<CourseTemplateWithCourseOutputDto>(courseTemplate);

                return ApiResponse<CourseTemplateWithCourseOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], courseTemplateWithCourseOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
