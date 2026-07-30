using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class EmailDigestBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailDigestBackgroundService> _logger;

        public EmailDigestBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<EmailDigestBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                   
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);

                    using var scope = _serviceProvider.CreateScope();
                    var notificationRepo = scope.ServiceProvider.GetRequiredService<IReadRepository<Notification>>();
                    var emailService = scope.ServiceProvider.GetService<IEmailService>();

                    if (emailService == null) continue;

                    var unreadNotifications = await notificationRepo
                        .GetWhere(n => !n.IsRead && n.CreatedAt >= DateTime.UtcNow.AddDays(-1))
                        .Include(n => n.User)
                        .ToListAsync(stoppingToken);

                    var groupedByUser = unreadNotifications.GroupBy(n => n.UserId);

                    foreach (var group in groupedByUser)
                    {
                        var user = group.FirstOrDefault()?.User;
                        if (user != null && !string.IsNullOrEmpty(user.Email))
                        {
                            var emailBody = $"<h3>Salam {user.UserName},</h3>" +
                                            $"<p>Son 24 saat ərzində oxunmamış {group.Count()} yeni bildirişiniz var:</p><ul>";

                            foreach (var notif in group)
                            {
                                emailBody += $"<li><b>{notif.Title}:</b> {notif.Message}</li>";
                            }

                            emailBody += "</ul>";

                            
                            await emailService.SendEmailAsync(user.Email, "Gündəlik Bildiriş Xülasəsi", emailBody);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Email Digest Background Job işləyərkən xəta baş verdi.");
                }
            }
        }
    }
}