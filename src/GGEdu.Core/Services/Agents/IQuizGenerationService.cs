namespace GGEdu.Core.Services.Agents
{
    public interface IQuizGenerationService
    {
        Task<string> GenerateQuizAsync(
            string courseCode,
            string teacherRequest,
            int totalQuestion,
            CancellationToken cancellationToken = default);
    }
}
