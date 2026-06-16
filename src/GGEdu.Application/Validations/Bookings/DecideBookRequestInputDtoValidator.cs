using FluentValidation;
using GGEdu.Core.DTOs.Courses.Bookings.Input;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;

namespace GGEdu.Application.Validations.Bookings
{
    public class DecideBookRequestInputDtoValidator : AbstractValidator<DecideBookRequestInputDto>
    {
        public DecideBookRequestInputDtoValidator(IStringLocalizer<SharedResources> _localizer)
        {
            RuleFor(c => c.StudentId)
                .NotEmpty().WithMessage(_localizer["Vld.BookingStudentIdCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.BookingStudentIdCantBeEmpty"]);

            RuleFor(c => c.IsAccepted)
                .NotEmpty().WithMessage(_localizer["Vld.BookingIsAcceptedCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.BookingIsAcceptedCantBeEmpty"]);
        }
    }
}
