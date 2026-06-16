using AutoMapper;
using AutoMapper.QueryableExtensions;
using GGEdu.Core.DTOs.Courses.Input;
using GGEdu.Core.DTOs.Courses.Output;
using GGEdu.Core.DTOs.Teachers.Output;
using GGEdu.Core.DTOs.Users.Outputs;
using GGEdu.Core.Entities.Teachers.Courses;
using GGEdu.Core.Repositories.Teachers;
using GGEdu.Core.Services.Teachers;
using GGEdu.Core.UnitOfWorks;
using GGEdu.Core.Utilities;
using GGEdu.Core.Utilities.Extensions;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Net;

namespace GGEdu.Application.Services.Teachers
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IStringLocalizer<SharedResources> _localizer;

        public CourseService(
            ICourseRepository courseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer)
        {
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<ApiResponse<bool>> CreateCourseAsync(
            CourseInputDto courseInputDto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var isExist = await _courseRepository.GetByAsync(c => c.Code.Equals(courseInputDto.Code.Trim().ToLower()));

                if (isExist != null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Crs.CourseCodeAlreadyExistError"], false);
                }

                var course = _mapper.Map<Course>(courseInputDto);

                await _courseRepository.CreateAsync(course);
                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(HttpStatusCode.Created, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> CreateCoursesAsync(
            List<CourseInputDto> courseInputDtos,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var codes = courseInputDtos.Select(c => c.Code.Trim().ToLower()).ToList();

                if(codes.GroupBy(x=>x).Any(g=>g.Count() > 1))
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Crs.DuplicateCodeRequestError"], false);
                }

                var isAnyExist = await _courseRepository.GetByAsync(c => codes.Contains(c.Code));

                if (isAnyExist != null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Crs.CourseCodeAlreadyExistError"], false);
                }

                var courses = _mapper.Map<List<Course>>(courseInputDtos);

                await _courseRepository.CreateRangeAsync(courses);
                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(HttpStatusCode.Created, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> DeleteCourseAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var course = await _courseRepository.GetByIdAsync(id, cancellationToken);

                if(course == null)
                {
                    return ApiResponse<bool>.ErrorResponse(
                        HttpStatusCode.NotFound, _localizer["Crs.CourseNotExistError"], default);
                }

                _courseRepository.Delete(course);
                await _unitOfWork.CommitAsync(cancellationToken);

                return ApiResponse<bool>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<List<CourseOutputDto>>> GetCoursesAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var courses = await _courseRepository.GetAllAsync(cancellationToken);

                var coursesOutputDtos = _mapper.Map<List<CourseOutputDto>>(courses);

                return ApiResponse<List<CourseOutputDto>>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], coursesOutputDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<CourseWithTeachersOutputDto>> GetCourseWithTeachersByCourseCodeAsync(
            string courseCode,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var course = await _courseRepository.GetCourseWithTeachersByCourseCodeAsync(courseCode, cancellationToken);

                if (course == null)
                {
                    return ApiResponse<CourseWithTeachersOutputDto>.ErrorResponse(
                        HttpStatusCode.BadRequest, _localizer["Crs.CourseNotExistError"], null);
                }

                var courseWithTeachersOutputDto = new CourseWithTeachersOutputDto()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Code = course.Code,
                    TeacherAllDetailsOutputDtos = course.TeacherCourses.Select(c =>
                    {
                        return new TeacherAllDetailsOutputDto
                        {
                            UserProfileDetail = _mapper.Map<UserProfileOutputDto>(c.Teacher.User),
                            TeacherProfileDetail = _mapper.Map<TeacherOutputDto>(c.Teacher),
                            TeacherCourseProfileDetail = _mapper.Map<TeacherCourseProfileOutputDto>(c),
                            TeacherLanguageProfileDetails = c.Teacher.TeacherLanguages.Select(x =>
                            {
                                return new TeacherLanguageOutputDto
                                {
                                    Id = x.Id,
                                    LanguageId = x.LanguageId,
                                    LanguageCode = x.Language.Code,
                                    LanguageLevel = x.Level,
                                    LanguageLevelName = x.Level.GetDescription()
                                };
                            }).ToList()
                        };
                    }).ToList()
                };

                return ApiResponse<CourseWithTeachersOutputDto>.SuccessResponse(
                    HttpStatusCode.OK, _localizer["Gnrl.Success"], courseWithTeachersOutputDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
