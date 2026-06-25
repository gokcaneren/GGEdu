using GGEdu.Core.DTOs.Emails.Input;

namespace GGEdu.Core.Services
{
    public interface IEmailNotificationService
    {
        Task SendEmailVerificationMail(
            EmailVerificationInputDto emailVerificationInputDto,
            CancellationToken cancellationToken = default);
    }
}
