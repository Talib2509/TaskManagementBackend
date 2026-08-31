using Microsoft.Extensions.Configuration;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using MimeKit;

using GmailMessage = Google.Apis.Gmail.v1.Data.Message;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            var clientId = GetRequiredConfigValue("GoogleMailSettings:ClientId");
            var clientSecret = GetRequiredConfigValue("GoogleMailSettings:ClientSecret");
            var refreshToken = GetRequiredConfigValue("GoogleMailSettings:RefreshToken");
            var fromEmail = GetRequiredConfigValue("GoogleMailSettings:FromEmail");
            var fromName = GetRequiredConfigValue("GoogleMailSettings:FromName");

            var credential = CreateUserCredential(clientId, clientSecret, refreshToken);

            var gmailService = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "CampusConnect"
            });

            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(fromName, fromEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(to));
            mimeMessage.Subject = subject;

            mimeMessage.Body = new TextPart("html")
            {
                Text = htmlBody
            };

            using var memoryStream = new MemoryStream();
            await mimeMessage.WriteToAsync(memoryStream);

            var rawMessage = Base64UrlEncode(memoryStream.ToArray());

            var gmailMessage = new GmailMessage
            {
                Raw = rawMessage
            };

            await gmailService.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();
        }

        private static UserCredential CreateUserCredential(
            string clientId,
            string clientSecret,
            string refreshToken)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            var tokenResponse = new TokenResponse
            {
                RefreshToken = refreshToken
            };

            var flow = new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets,
                    Scopes = new[]
                    {
                    GmailService.Scope.GmailSend
                    }
                });

            return new UserCredential(flow, "CampusConnectUser", tokenResponse);
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        private string GetRequiredConfigValue(string key)
        {
            var value = _configuration[key]?.Trim().Trim('"');

            if (string.IsNullOrWhiteSpace(value))
                throw new Exception($"{key} tapılmadı.");

            var envValue = _configuration[value];

            var result = string.IsNullOrWhiteSpace(envValue)
                ? value
                : envValue.Trim().Trim('"');

            if (result == value && value.StartsWith("GOOGLE_"))
            {
                throw new InvalidOperationException($"{value} üçün configuration dəyəri tapılmadı.");
            }

            return result;
        }
    }
}
