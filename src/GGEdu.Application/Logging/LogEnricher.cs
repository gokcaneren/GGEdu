using GGEdu.Core.Logging;
using Serilog.Core;
using Serilog.Events;

namespace GGEdu.Application.Logging
{
    public class LogEnricher<T> : ILogEventEnricher where T : MainLogFormat
    {

        private readonly T _logFormat;

        public LogEnricher(T logFormat)
        {
            _logFormat = logFormat;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var logProperties = _logFormat.GetType().GetProperties();

            foreach (var property in logProperties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(_logFormat, null);

                if (propertyValue != null)
                {
                    logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(propertyName, propertyValue));
                }
            }
        }
    }
}
