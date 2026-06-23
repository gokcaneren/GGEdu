using GGEdu.Core.Repositories.Teachers;
using GGEdu.Infrastructure.Agents.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace GGEdu.Infrastructure.Extensions
{
    public static class KernelFactory
    {
        public static Kernel BuildKernel(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var builder = Kernel.CreateBuilder();

            builder.AddAzureOpenAIChatCompletion(
                deploymentName: configuration["AzureOpenAI:Deployment"]!,
                endpoint: configuration["AzureOpenAI:Endpoint"]!,
                apiKey: configuration["AzureOpenAI:ApiKey"]!);

            builder.Plugins.AddFromObject(
                new CoursePlugin(serviceProvider.GetRequiredService<ITeacherCourseRepository>()),
                nameof(CoursePlugin));

            return builder.Build();
        }
    }
}
