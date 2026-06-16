using FluentValidation;
using GGEdu.Core.DTOs.Subscriptions.Input;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;

namespace GGEdu.Application.Validations.Subscriptions
{
    public class SubRequestDecideInputDtoValidator : AbstractValidator<SubRequestDecideInputDto>
    {
        public SubRequestDecideInputDtoValidator(IStringLocalizer<SharedResources> _localizer)
        {
            RuleFor(c => c.StudentId).
                NotEmpty().WithMessage(_localizer["Vld.SubsStudentIdCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.SubsStudentIdCantBeEmpty"]);

            RuleFor(c => c.IsAccepted).
                NotEmpty().WithMessage(_localizer["Vld.SubAcceptCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.SubAcceptCantBeEmpty"]);
        }
    }
}
