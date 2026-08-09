using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Infrastructure.HealthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IWebHostEnvironment _env;

        public StorageHealthCheck(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadPath = Path.Combine(webRoot, "uploads");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Write test file
                var testFile = Path.Combine(uploadPath, $".health_check_{Guid.NewGuid():N}.tmp");
                File.WriteAllText(testFile, "health-check-ok");
                File.Delete(testFile);

                return Task.FromResult(HealthCheckResult.Healthy("Fayl saxlanc qovluğu (Storage) aktivdir və yazma icazəsi var."));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Fayl saxlanc qovluğuna giriş zamanı xəta:", ex));
            }
        }
    }
}
