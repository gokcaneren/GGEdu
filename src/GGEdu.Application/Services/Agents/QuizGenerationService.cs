using GGEdu.Core.Repositories.Teachers;
using GGEdu.Core.Services.Agents;
using GGEdu.Core.Services.Users;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

namespace GGEdu.Application.Services.Agents
{
    public class QuizGenerationService : IQuizGenerationService
    {
        private readonly Kernel _kernel;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITeacherRepository _teacherRepository;

        public QuizGenerationService(
            Kernel kernel,
            ICurrentUserService currentUserService,
            ITeacherRepository teacherRepository)
        {
            _kernel = kernel;
            _currentUserService = currentUserService;
            _teacherRepository = teacherRepository;
        }

        public async Task<string> GenerateQuizAsync(
            string courseCode,
            string teacherRequest,
            int totalQuestionCount,
            CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;

            var teacher = await _teacherRepository.GetByAsync(c => c.UserId.Equals(userId), cancellationToken);

            if (teacher == null)
            {
                return null;
            }

            var settings = new AzureOpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            var result = await _kernel.InvokePromptAsync(
                $"""
                You're a teacher assistant! Just create the questions about course!
                TeacherId:{teacher.Id}, CourseCode:{courseCode}
                Firs use get_course function to get course and
                check there is a connection between course and {teacherRequest}
                If there is a connection, create {totalQuestionCount} question about {teacherRequest} and Course.
                """,
                new KernelArguments(settings),
                cancellationToken: cancellationToken);

            return result.ToString();
        }
    }
}
