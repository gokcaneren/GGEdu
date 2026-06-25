using GGEdu.Core.DTOs.Emails.Input;
using GGEdu.Core.Services;
using GGEdu.Core.Utilities;
using System.Text;
using System.Text.Json;

namespace GGEdu.Application.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly N8NSettings _n8nSettings;

        public EmailNotificationService(
            IHttpClientFactory httpClientFactory,
            N8NSettings n8nSettings)
        {
            _httpClientFactory = httpClientFactory;
            _n8nSettings = n8nSettings;
        }

        public async Task SendEmailVerificationMail(
            EmailVerificationInputDto emailVerificationInputDto,
            CancellationToken cancellationToken = default)
        {
            await SendToN8NAsync(_n8nSettings.EmailVerificationPath, emailVerificationInputDto, cancellationToken);
        }

        private async Task SendToN8NAsync<T>(
            string path,
            T payload,
            CancellationToken cancellationToken)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("N8N");

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(path, content, cancellationToken);

                //if (!response.IsSuccessStatusCode)
                //{
                //    var body = await response.Content.ReadAsStringAsync(cancellationToken);
                //    _logger.LogWarning(
                //        "N8N webhook call failed. Path: {Path} StatusCode: {StatusCode} Body: {Body}",
                //        path, response.StatusCode, body);
                //}
                //else
                //{
                //    _logger.LogInformation(
                //        "N8N webhook called successfully. Path: {Path}",
                //        path);
                //}
            }
            catch (HttpRequestException ex)
            {
                //_logger.LogError(ex,
                //    "N8N webhook HTTP request failed. Path: {Path}",
                //    path);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                //_logger.LogError(ex,
                //    "N8N webhook request timed out. Path: {Path}",
                //    path);
            }
        }
    }
}
