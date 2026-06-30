using GGEdu.Core.Constants;
using GGEdu.Core.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GGEdu.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection BuildHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var n8nSettings = configuration.GetSection(nameof(N8NSettings)).Get<N8NSettings>();

            services.AddSingleton<N8NSettings>();

            services.AddHttpClient(HttpClientConstants.N8N, client =>
            {
                client.BaseAddress = new Uri(n8nSettings!.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services;
        }
    }
}
