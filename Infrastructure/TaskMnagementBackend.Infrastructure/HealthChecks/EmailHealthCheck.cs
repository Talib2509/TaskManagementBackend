using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Infrastructure.HealthChecks
{
    public class EmailHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public EmailHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var clientId = _configuration["GoogleMailSettings:ClientId"];
                var clientSecret = _configuration["GoogleMailSettings:ClientSecret"];
                var refreshToken = _configuration["GoogleMailSettings:RefreshToken"];

                if (string.IsNullOrWhiteSpace(clientId) ||
                    string.IsNullOrWhiteSpace(clientSecret) ||
                    string.IsNullOrWhiteSpace(refreshToken))
                {
                    return Task.FromResult(HealthCheckResult.Degraded("Email konfiqurasiyası natamamdır və ya defolt dəyərlər təyin edilib."));
                }

                return Task.FromResult(HealthCheckResult.Healthy("Email servisi konfiqurasiyası qaydasındadır."));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Email servisi yoxlanışı zamanı xəta:", ex));
            }
        }
    }
}
