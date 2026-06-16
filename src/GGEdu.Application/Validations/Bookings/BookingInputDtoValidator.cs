using FluentValidation;
using GGEdu.Core.DTOs.Courses.Bookings.Input;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;

namespace GGEdu.Application.Validations.Bookings
{
    public class BookingInputDtoValidator : AbstractValidator<BookingInputDto>
    {
        public BookingInputDtoValidator(IStringLocalizer<SharedResources> _localizer)
        {
            RuleFor(c => c.TeacherId)
                .NotEmpty().WithMessage(_localizer["Vld.BookingAvailabilityCourseSlotIdCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.BookingAvailabilityCourseSlotIdCantBeEmpty"]);

            RuleFor(c => c.AvailabilityCourseSlotId)
                .NotEmpty().WithMessage(_localizer["Vld.BookingTeacherIdCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.BookingTeacherIdCantBeEmpty"]);
        }
    }
}
