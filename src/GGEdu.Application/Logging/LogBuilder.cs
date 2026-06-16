using GGEdu.Core.Logging;
using GGEdu.Core.Logging.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;

namespace GGEdu.Application.Logging
{
    public static class LogBuilder
    {
        public static IServiceCollection BuildLogger(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Elasticsearch(CreateElasticSettings(configuration))
                .Enrich.FromLogContext()
                .CreateLogger();

            services.AddSerilog();

            services.AddSingleton<ILogger>(Log.Logger);

            return services;
        }

        public static ElasticsearchSinkOptions CreateElasticSettings(IConfiguration configuration)
        {
            var elasticSearchOptions = configuration.GetSection("ElasticSearch").Get<ElasticSearch>();

            var logEventLevel = Enum.TryParse<LogEventLevel>(elasticSearchOptions.LogLevel, true, out var parsedLevel)
                                ? parsedLevel
                                : LogEventLevel.Information;

            var failurePath = Path.Combine(AppContext.BaseDirectory, "Logs", elasticSearchOptions.FailureFile);

            var elasticSearchSinkOptions = new ElasticsearchSinkOptions(new Uri($"{elasticSearchOptions.Url}"))
            {
                MinimumLogEventLevel = logEventLevel,
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                IndexFormat = elasticSearchOptions.Index,
                FailureSink = new FileSink(failurePath, new JsonFormatter(renderMessage: true), null),
                EmitEventFailure = EmitEventFailureHandling.WriteToFailureSink | EmitEventFailureHandling.RaiseCallback,
                FailureCallback = (logEvent, exception) => Console.WriteLine($"Elastic write failure: {exception}")
            };

            return elasticSearchSinkOptions;
        }
    }
}
