using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Infrastructure.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly AppDbContext _dbContext;

        public DatabaseHealthCheck(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
                if (canConnect)
                {
                    return HealthCheckResult.Healthy("Verilənlər bazası bağlantısı aktivdir.");
                }

                return HealthCheckResult.Unhealthy("Verilənlər bazasına qoşulmaq mümkün olmadı.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Verilənlər bazası yoxlanışı zamanı xəta:", ex);
            }
        }
    }
}
