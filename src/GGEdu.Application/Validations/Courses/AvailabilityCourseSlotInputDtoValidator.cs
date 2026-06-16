using FluentValidation;
using GGEdu.Core.DTOs.Courses.AvailabilityCourseSlot.Input;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;

namespace GGEdu.Application.Validations.Courses
{
    public class AvailabilityCourseSlotInputDtoValidator : AbstractValidator<AvailabilityCourseSlotInputDto>
    {
        public AvailabilityCourseSlotInputDtoValidator(IStringLocalizer<SharedResources> _localizer)
        {
            RuleFor(c => c.CourseTemplateId)
                .NotEmpty().WithMessage(_localizer["Vld.CourseSlotCourseTemplateIdCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseSlotCourseTemplateIdCantBeEmpty"]);

            RuleFor(c => c.FromDate)
                .NotEmpty().WithMessage(_localizer["Vld.CourseSlotFromDateCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseSlotFromDateCantBeEmpty"]);

            RuleFor(c => c.ToDate)
                .NotEmpty().WithMessage(_localizer["Vld.CourseSlotToDateCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.CourseSlotToDateCantBeEmpty"]);
        }
    }
}
