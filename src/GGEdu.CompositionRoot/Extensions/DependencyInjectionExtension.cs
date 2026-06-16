using GGEdu.CompositionRoot.Attributes;
using GGEdu.CompositionRoot.Enums;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GGEdu.CompositionRoot.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddCustomDependencies(this IServiceCollection services, List<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                var implementations = types
                    .Where(t => (t.IsClass ) && (t.Name.EndsWith("Repository") || t.Name.EndsWith("Service")
                    || t.Name.EndsWith("UnitOfWork")))
                    .ToList();

                foreach (var implementation in implementations)
                {
                    var serviceInterface = implementation.GetInterfaces()
                        .FirstOrDefault(i => i.Name == $"I{implementation.Name}");

                    if (serviceInterface != null)
                    {
                        var lifeTimeAttr = implementation.GetCustomAttribute<LifeTimeAttribute>();
                        var lifeTime = lifeTimeAttr?.LifetimeType ?? ServiceLifetimeType.Scoped;

                        switch (lifeTime)
                        {
                            case ServiceLifetimeType.Singleton:
                                services.AddSingleton(serviceInterface, implementation);
                                break;
                            case ServiceLifetimeType.Scoped:
                                services.AddScoped(serviceInterface, implementation);
                                break;
                            case ServiceLifetimeType.Transient:
                                services.AddTransient(serviceInterface, implementation);
                                break;
                        }
                    }

                }
            }

            return services;
        } 
    }
}
