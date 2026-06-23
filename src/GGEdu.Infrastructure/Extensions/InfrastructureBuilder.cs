using GGEdu.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.Reflection;

namespace GGEdu.Infrastructure.Extensions
{
    public static class InfrastructureBuilder
    {
        public static IServiceCollection BuildInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GGEduContext>((serviceProvider, options) =>
                options.UseNpgsql(configuration["ConnectionStrings:GGEduDb"], c =>
                c.MigrationsAssembly(Assembly.GetAssembly(typeof(GGEduContext))!.GetName().Name)));

            services.AddScoped<Kernel>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                return KernelFactory.BuildKernel(sp, configuration);
            });

            return services;
        }
    }
}
