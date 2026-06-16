using GGEdu.Application.Logging;
using GGEdu.Core.Constants;
using GGEdu.Core.Logging;
using GGEdu.Core.Utilities;
using GGEdu.Localization.Resources;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace GGEdu.API.Middlewares
{
    public class LoggingMiddeware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public LoggingMiddeware(
            RequestDelegate next,
            Serilog.ILogger logger,
            IStringLocalizer<SharedResources> localizer)
        {
            _next = next;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals(PathContants.LoginPath))
            {
                await _next.Invoke(context);
                return;
            }
            var infoLog = await CreateInformationLogAsync(context);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();

            try
            {
                context.Response.Body = responseBody;

                await _next.Invoke(context);

                infoLog.ResponseStatusCode = context.Response.StatusCode;
                infoLog.ResponseBody = await ReadResponseBodyAsync(context.Response);

                await responseBody.CopyToAsync(originalBodyStream);

                _logger.LogInformation<InformationLogFormat>(infoLog);
            }
            catch (Exception ex)
            {
                context.Response.Body = originalBodyStream;

                var errorLog = await CreateErrorLogAsync(context, ex.Message, ex.StackTrace);
                _logger.LogError<ErrorLogFormat>(errorLog);

                await WriteErrorResponseAsync(context, ex);
            }
        }

        private async Task WriteErrorResponseAsync(HttpContext context, Exception ex)
        {
            // Response zaten yazılmaya başlandıysa müdahale etme
            if (context.Response.HasStarted) return;

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.ErrorResponse(
                HttpStatusCode.InternalServerError,
                _localizer["Gnrl.ServerError"],
                null
            );

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest httpRequest)
        {
            var requestContent = string.Empty;

            httpRequest.EnableBuffering();
            using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8, true, 1024, true))
            {
                requestContent = await reader.ReadToEndAsync();
            }
            httpRequest.Body.Position = 0;

            return requestContent;
        }


        private async Task<string> ReadResponseBodyAsync(HttpResponse httpResponse)
        {
            httpResponse.Body.Seek(0, SeekOrigin.Begin);
            var responseContent = await new StreamReader(httpResponse.Body).ReadToEndAsync();
            httpResponse.Body.Seek(0, SeekOrigin.Begin);

            return responseContent;
        }


        private async Task<InformationLogFormat> CreateInformationLogAsync(HttpContext httpContext)
        {
            var informationLog = new InformationLogFormat()
            {
                HttpMethod = httpContext.Request.Method,
                RequestPath = httpContext.Request.Path,
                RequestBody = await ReadRequestBodyAsync(httpContext.Request)
            };

            return informationLog;
        }

        private async Task<ErrorLogFormat> CreateErrorLogAsync(
            HttpContext httpContext,
            string exceptionMessage,
            string exceptionStackTrace)
        {
            var errorLog = new ErrorLogFormat()
            {
                HttpMethod = httpContext.Request.Method,
                RequestPath = httpContext.Request.Path,
                RequestBody = await ReadRequestBodyAsync(httpContext.Request),
                ExceptionMessage = exceptionMessage,
                ExceptionStackTrace = exceptionStackTrace
            };

            return errorLog;
        }
    }
}
