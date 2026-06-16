using FluentValidation;
using GGEdu.Core.DTOs.Courses.CourseTemplates.Input;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;

namespace GGEdu.Application.Validations.Courses
{
    public class CourseTemplateInputDtoValidator : AbstractValidator<CourseTemplateInputDto>
    {
        public CourseTemplateInputDtoValidator(IStringLocalizer<SharedResources> _localizer)
        {
            RuleFor(c => c.TeacherCourseId)
                .NotEmpty().WithMessage(_localizer["Vld.CourseTemplateTeacherCourseIdCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseTemplateTeacherCourseIdCantBeEmpty"]);

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(_localizer["Vld.CourseTemplateNameCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseTemplateNameCantBeEmpty"]);

            RuleFor(c => c.DayOfWeek)
                .NotEmpty().WithMessage(_localizer["Vld.CourseTemplateDayOfWeekCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseTemplateDayOfWeekCantBeEmpty"]);

            RuleFor(c => c.StartLocalTime)
                .NotEmpty().WithMessage(_localizer["Vld.CourseTemplateStartLocalTimeCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseTemplateStartLocalTimeCantBeEmpty"]);

            RuleFor(c => c.EndLocalTime)
                .NotEmpty().WithMessage(_localizer["Vld.CourseTemplateEndLocalTimeCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseTemplateEndLocalTimeCantBeEmpty"]);

            RuleFor(c => c.TimeZoneId)
                .NotEmpty().WithMessage(_localizer["Vld.CourseTemplateTimeZoneIdCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseTemplateTimeZoneIdCantBeEmpty"]);

            RuleFor(c => c.AutoGenerateSlots)
                .NotEmpty().WithMessage(_localizer["Vld.CourseTemplateAutoGenerateSlotsCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseTemplateAutoGenerateSlotsCantBeEmpty"]);

            RuleFor(c => c.GenerateDaysAhead)
                .NotEmpty().WithMessage(_localizer["Vld.CourseTemplateGenerateDaysAheadCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseTemplateGenerateDaysAheadCantBeEmpty"]);

            RuleFor(c => c.IsActive)
                .NotEmpty().WithMessage(_localizer["Vld.CourseTemplateIsActiveCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseTemplateIsActiveCantBeEmpty"]);
        }
    }
}
