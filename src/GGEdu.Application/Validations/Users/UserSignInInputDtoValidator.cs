using FluentValidation;
using GGEdu.Core.DTOs.Users.Inputs;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;

namespace GGEdu.Application.Validations.Users
{
    public class UserSignInInputDtoValidator : AbstractValidator<UserSignInInputDto>
    {
        public UserSignInInputDtoValidator(IStringLocalizer<SharedResources> _localizer)
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage(_localizer["Vld.UserEmailCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.UserEmailCantBeEmpty"])
                .EmailAddress().WithMessage(_localizer["Vld.UserEmailFormat"]);

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage(_localizer["Vld.UserPasswordCantBeEmpty"])
                .NotNull().WithMessage(_localizer["Vld.UserPasswordCantBeEmpty"]);
        }
    }
}
