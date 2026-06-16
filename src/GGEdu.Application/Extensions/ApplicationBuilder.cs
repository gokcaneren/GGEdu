using FluentValidation;
using GGEdu.Application.Helpers;
using GGEdu.Application.Validations.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GGEdu.Application.Extensions
{
    public static class ApplicationBuilder
    {
        public static IServiceCollection BuildApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<TokenConfiguration>();

            services.AddAutoMapper(opt => { }, Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssemblyContaining(typeof(UserRegisterInputDtoValidator));

            services.AddHttpContextAccessor();

            services.BuildAuthenticationSystem(configuration);

            return services;
        }

    }
}
