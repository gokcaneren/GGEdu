using GGEdu.CompositionRoot.Extensions;
using GGEdu.Core.Repositories;
using GGEdu.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GGEdu.CompositionRoot.DependencyInjection
{
    public static class DIBuilder
    {
        public static IServiceCollection BuidDIServices(this IServiceCollection services, List<Assembly> assemblies)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return DependencyInjectionExtension.AddCustomDependencies(services, assemblies);
        }

    }
}
