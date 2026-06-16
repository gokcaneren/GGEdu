using FluentValidation;
using GGEdu.Core.DTOs.Teachers.Input;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;

namespace GGEdu.Application.Validations.Teachers
{
    public class TeacherCourseInputDtoValidator : AbstractValidator<TeacherCourseInputDto>
    {
        public TeacherCourseInputDtoValidator(IStringLocalizer<SharedResources> _localizer)
        {
            RuleFor(c => c.Price)
                .NotNull().WithMessage(_localizer["Vld.TeacherCoursePriceCantBeEmpty"])
                .NotEmpty().WithMessage(_localizer["Vld.TeacherCoursePriceCantBeEmpty"])
                .GreaterThan(0).WithMessage(_localizer["Vld.TeacherCoursePrıceMustBeBıggerThanZero"]);

            RuleFor(c => c.DurationMinutes)
                .NotNull().WithMessage(_localizer["Vld.TeacherCourseDurationMınutesCantBeEmpty"])
                .NotEmpty().WithMessage(_localizer["Vld.TeacherCourseDurationMınutesCantBeEmpty"])
                .GreaterThan(10).WithMessage(_localizer["Vld.TeacherCourseDurationMustBeBiggerThanTenMinutes"]);

            RuleFor(c => c.Currency)
                .NotNull().WithMessage(_localizer["Vld.TeacherCourseCurrencyCantBeEmpty"])
                .NotEmpty().WithMessage(_localizer["Vld.TeacherCourseCurrencyCantBeEmpty"]);

            RuleFor(c => c.CourseId)
                .NotNull().WithMessage(_localizer["Vld.TeacherCourseCourseIdCantBeEmpty"])
                .NotEmpty().WithMessage(_localizer["Vld.TeacherCourseCourseIdCantBeEmpty"]);
        }
    }
}
