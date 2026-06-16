using GGEdu.Core.Logging;
using Serilog;
using System.Text.Json;

namespace GGEdu.Application.Logging
{
    public static class LogFormatExtension
    {
        public static void LogInformation<T>(this ILogger logger, T logModel) where T : MainLogFormat
        {
            //var jsonLog = JsonSerializer.Serialize(logModel);
            logger.ForContext(new LogEnricher<T>(logModel))
                .Information("Request Log");
        }

        public static void LogError<T>(this ILogger logger, T logModel) where T : MainLogFormat
        {
            //var jsonLog = JsonSerializer.Serialize(logModel);
            logger.ForContext(new LogEnricher<T>(logModel))
                .Error("Error Log");
        }
    }
}
