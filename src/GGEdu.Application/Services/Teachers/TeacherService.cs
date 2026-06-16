using AutoMapper;
using GGEdu.Core.DTOs.Teachers.Input;
using GGEdu.Core.DTOs.Teachers.Output;
using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Entities.Teachers.Languages;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Core.Repositories.Users;
using GGEdu.Core.Services.Teachers;
using GGEdu.Core.Services.Users;
using GGEdu.Core.UnitOfWorks;
using GGEdu.Core.Utilities;
using GGEdu.Core.Utilities.Extensions;
using GGEdu.Infrastructure.Extensions;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace GGEdu.Application.Services.Teachers
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITeacherCourseRepository _teacherCourseRepository;
        private readonly ITeacherLanguageRepository _teacherLanguageRepository;

        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResources> _localizer;

        public TeacherService(
            ITeacherRepository teacherRepository,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            ITeacherCourseRepository teacherCourseRepository,
            ITeacherLanguageRepository teacherLanguageRepository)
        {
            _teacherRepository = teacherRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _teacherCourseRepository = teacherCourseRepository;
            _teacherLanguageRepository = teacherLanguageRepository;
        }

        public async Task<ApiResponse<bool>> DeleteTeacherCourseByTeacherCourseId(
            Guid teacherCourseId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

                if (user == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Auth.UserCantBeFound"], false);
                }

                var teacher = await _teacherRepository.GetTeacherDetailByUserIdAsync(userId, cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.NotAssignedAsTeacher"], false);
                }

                var teacherCourse = teacher.TeacherCourses.FirstOrDefault(c => c.Id.Equals(teacherCourseId));

                if (teacherCourse == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcrcrs.TeacherCourseNotFoundError"], false);
                }

                _teacherCourseRepository.Delete(teacherCourse);
                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<List<TeacherCourseOutputDto>>> GetTeacherCoursesAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetTeacherDetailByUserIdAsync(userId, cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<List<TeacherCourseOutputDto>>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.NotAssignedAsTeacher"], null);
                }

                var teacherCourseOutputDtos = _mapper.Map<List<TeacherCourseOutputDto>>(teacher.TeacherCourses);

                return ApiResponse<List<TeacherCourseOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], teacherCourseOutputDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<TeacherDetailOutputDto>> GetTeacherDetailsAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

                if (user == null)
                {
                    return ApiResponse<TeacherDetailOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Auth.UserCantBeFound"], null);
                }

                var teacher = await _teacherRepository.GetTeacherDetailByUserIdAsync(userId, cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<TeacherDetailOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.NotAssignedAsTeacher"], null);
                }

                var teacherDetailOutputDto = _mapper.Map<TeacherDetailOutputDto>(teacher);

                return ApiResponse<TeacherDetailOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], teacherDetailOutputDto);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<TeacherDetailOutputDto>> GetTeacherDetailsByTeacherIdAsync(
            Guid teacherId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var teacher = await _teacherRepository.GetTeacherDetailByTeacherIdAsync(teacherId, cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<TeacherDetailOutputDto>.ErrorResponse(
                         HttpStatusCode.NotFound, _localizer["Tcr.TeacherCantBeFound"], null);
                }

                var teacherDetailOutputDto = _mapper.Map<TeacherDetailOutputDto>(teacher);

                return ApiResponse<TeacherDetailOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], teacherDetailOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<List<TeacherLanguageOutputDto>>> GetTeacherLanguagesByUserIdAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetTeacherWithLanguagesByUserIdAsync(
                    userId, cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<List<TeacherLanguageOutputDto>>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.NotAssignedAsTeacher"], null);
                }

                var teacherLanguageOutputDtos = teacher.TeacherLanguages.Select(c =>
                {
                    return new TeacherLanguageOutputDto
                    {
                        Id = c.Id,
                        LanguageId = c.LanguageId,
                        LanguageCode = c.Language.Code,
                        LanguageLevel = c.Level,
                        LanguageLevelName = c.Level.GetDescription()
                    };
                }).ToList();

                return ApiResponse<List<TeacherLanguageOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], teacherLanguageOutputDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<TeacherOutputDto>> GetTeacherProfileAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

                if (user == null)
                {
                    return ApiResponse<TeacherOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Auth.UserCantBeFound"], null);
                }

                var teacher = await _teacherRepository.GetByAsync(c => c.UserId.Equals(user.Id), cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<TeacherOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.NotAssignedAsTeacher"], null);
                }

                var teacherOutputDto = _mapper.Map<TeacherOutputDto>(teacher);

                return ApiResponse<TeacherOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], teacherOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<TeacherOutputDto>> GetTeacherProfileByTeacherIdAsync(
            Guid teacherId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var teacher = await _teacherRepository.GetByIdAsync(teacherId, cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<TeacherOutputDto>.ErrorResponse(
                         HttpStatusCode.NotFound, _localizer["Tcr.TeacherCantBeFound"], null);
                }

                var teacherOutputDto = _mapper.Map<TeacherOutputDto>(teacher);

                return ApiResponse<TeacherOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], teacherOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<List<TeacherLanguageOutputDto>>> UpdateTeacherLanguageAsync(
            List<TeacherLanguageInputDto> teacherLanguageInputDtos,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;

                var teacher = await _teacherRepository.GetTeacherWithLanguagesByUserIdAsync(
                    userId, cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<List<TeacherLanguageOutputDto>>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.NotAssignedAsTeacher"], null);
                }
                //TODO: Burda TeacherLanguageId gerekli mi kontrol edilecek. Eger gerekli ise ona gore islem yapilacak.
                if (!teacherLanguageInputDtos.IsNullOrEmpty())
                {
                    var deletedLanguages = teacher.TeacherLanguages.Where(c => !teacherLanguageInputDtos.Any(x =>
                    c.LanguageId.Equals(x.LanguageId))).ToList();

                    _teacherLanguageRepository.DeleteRange(deletedLanguages);

                    foreach (var languageInputDto in teacherLanguageInputDtos)
                    {
                        var existLanguage = teacher.TeacherLanguages.FirstOrDefault(c =>
                        c.LanguageId.Equals(languageInputDto.LanguageId));

                        if(existLanguage != null)
                        {
                            existLanguage.Level = languageInputDto.LanguageLevel;
                        }
                        else
                        {
                            var newLanguage = new TeacherLanguage()
                            {
                                TeacherId = teacher.Id,
                                LanguageId = languageInputDto.LanguageId,
                                Level = languageInputDto.LanguageLevel
                            };

                            teacher.TeacherLanguages.Add(newLanguage);
                        }
                    }
                }
                else
                {
                    _teacherLanguageRepository.DeleteRange(teacher.TeacherLanguages.ToList());
                }

                await _unitOfWork.CommitAsync(cancellationToken);

                var updatedTeacher = await _teacherRepository.GetTeacherWithLanguagesByUserIdAsync(userId,
                    cancellationToken);

                var teacherLanguageOutputDtos = teacher.TeacherLanguages.Select(c =>
                {
                    return new TeacherLanguageOutputDto
                    {
                        Id = c.Id,
                        LanguageId = c.LanguageId,
                        LanguageCode = c.Language.Code,
                        LanguageLevel = c.Level,
                        LanguageLevelName = c.Level.GetDescription()
                    };
                }).ToList();

                return ApiResponse<List<TeacherLanguageOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], teacherLanguageOutputDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<TeacherDetailOutputDto>> UpdateTeacherProfileAsync(
            TeacherProfileUpdateInputDto teacherProfileUpdateInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // TODO: Burada ileride açılan ders ile ders çeşidi arasında bağlantı yapıldığında güncelleme gerekli. Bir ders çesidi
                // iptal edilirken o ders çeşidi ile bir ders booklanmış mı bakılması gerekir!!
                var userId = _currentUserService.UserId;

                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

                if (user == null)
                {
                    return ApiResponse<TeacherDetailOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Auth.UserCantBeFound"], null);
                }

                var teacher = await _teacherRepository.GetTeacherDetailByUserIdAsync(userId, cancellationToken);

                if (teacher == null)
                {
                    return ApiResponse<TeacherDetailOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Tcr.NotAssignedAsTeacher"], null);
                }

                teacher.Bio = teacherProfileUpdateInputDto.Bio;
                teacher.DisplayName = teacherProfileUpdateInputDto.DisplayName;

                //TODO: Budara TeacherCourseId alinmasi gerekli mi incelenecek. Sadece CourseId yeterli ise ona gore islem yapilacak!
                if (!teacherProfileUpdateInputDto.TeacherCourses.IsNullOrEmpty())
                {
                    var deletedCourses = teacher.TeacherCourses.Where(c => !teacherProfileUpdateInputDto.TeacherCourses.Any(x =>
                    c.Id.Equals(x.Id) || c.CourseId.Equals(x.CourseId))).ToList();

                    _teacherCourseRepository.DeleteRange(deletedCourses);

                    foreach (var course in teacherProfileUpdateInputDto.TeacherCourses)
                    {
                        if (course.Id.HasValue)
                        {
                            var existCourse = teacher.TeacherCourses.FirstOrDefault(c => c.Id.Equals(course.Id));

                            if (existCourse == null)
                                continue;

                            existCourse.Price = course.Price;
                            existCourse.DurationMinutes = course.DurationMinutes;
                            existCourse.Currency = course.Currency;
                        }
                        else
                        {
                            var existCourse = teacher.TeacherCourses.FirstOrDefault(c => c.CourseId.Equals(course.CourseId));
                            if (existCourse != null)
                            {
                                existCourse.Price = course.Price;
                                existCourse.DurationMinutes = course.DurationMinutes;
                                existCourse.Currency = course.Currency;
                            }
                            else
                            {
                                var newCourse = new TeacherCourse
                                {
                                    TeacherId = teacher.Id,
                                    CourseId = course.CourseId,
                                    Price = course.Price,
                                    Currency = course.Currency,
                                    DurationMinutes = course.DurationMinutes
                                };

                                teacher.TeacherCourses.Add(newCourse);
                            }
                        }
                    }
                }
                else
                {
                    _teacherCourseRepository.DeleteRange(teacher.TeacherCourses.ToList());
                }

                await _unitOfWork.CommitAsync(cancellationToken);

                var updatedTeacher = await _teacherRepository.GetTeacherDetailByUserIdAsync(userId, cancellationToken);

                var teacherDetailOutputDto = _mapper.Map<TeacherDetailOutputDto>(updatedTeacher);

                return ApiResponse<TeacherDetailOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], teacherDetailOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
