using FluentValidation;
using GGEdu.Core.DTOs.Users.Inputs;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;

namespace GGEdu.Application.Validations.Users
{
    public class UserRegisterInputDtoValidator : AbstractValidator<UserRegisterInputDto>
    {
        public UserRegisterInputDtoValidator(IStringLocalizer<SharedResources> _localizer)
        {
            RuleFor(c => c.Email)
                .NotNull().WithMessage(_localizer["Vld.UserEmailCantBeEmpty"])
                .NotEmpty().WithMessage(_localizer["Vld.UserEmailCantBeEmpty"])
                .EmailAddress().WithMessage(_localizer["Vld.UserEmailFormat"]);

            RuleFor(c => c.Password)
                .NotNull().WithMessage(_localizer["Vld.UserPasswordCantBeEmpty"])
                .NotEmpty().WithMessage(_localizer["Vld.UserPasswordCantBeEmpty"])
                .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[.!?\-_&%/(]).{6,}$").WithMessage("Vld.UserPasswordFormat");

            RuleFor(c => c.FirstName)
                .NotNull().WithMessage(_localizer["Vld.UserFirstNameCantBeEmpty"])
                .NotEmpty().WithMessage(_localizer["Vld.UserFirstNameCantBeEmpty"]);

            RuleFor(c => c.LastName)
                .NotNull().WithMessage(_localizer["Vld.UserLastNameCantBeEmpty"])
                .NotEmpty().WithMessage(_localizer["Vld.UserLastNameCantBeEmpty"]);
        }
    }
}
