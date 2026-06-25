namespace GGEdu.Core.DTOs.Emails.Input
{
    public record EmailVerificationInputDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Token { get; set; }
    }
}
