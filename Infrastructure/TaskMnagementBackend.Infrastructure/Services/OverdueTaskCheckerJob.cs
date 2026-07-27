using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartTask.Domain.Entities;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class OverdueTaskCheckerJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OverdueTaskCheckerJob> _logger;

        public OverdueTaskCheckerJob(
            IServiceScopeFactory scopeFactory,
            ILogger<OverdueTaskCheckerJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OverdueTaskCheckerJob başladı.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var taskReadRepository = scope.ServiceProvider.GetRequiredService<IReadRepository<ProjectTask>>();
                        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                        var now = DateTime.UtcNow;

                        // Находим задачи с просроченным дедлайном, которые ещё не завершены
                        var overdueTasks = await taskReadRepository
                            .GetWhere(x => x.Deadline.HasValue
                                        && x.Deadline.Value < now
                                        && x.Status != TaskStatus.Completed
                                        && x.Status != TaskStatus.Blocked)
                            .ToListAsync(stoppingToken);

                        foreach (var task in overdueTasks)
                        {
                            await notificationService.CreateAsync(
                                new Notification
                                {
                                    UserId = task.UserId,
                                    Message = $"Müddəti keçmiş tapşırıq: \"{task.Title}\" (Son tarix: {task.Deadline:dd.MM.yyyy HH:mm})."
                                },
                                stoppingToken);
                        }

                        if (overdueTasks.Any())
                        {
                            _logger.LogInformation($"{overdueTasks.Count} müddəti keçmiş tapşırıq üçün bildiriş göndərildi.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Müddəti keçmiş tapşırıqlar yoxlanılarkən xəta baş verdi.");
                }

                // Проверка каждые 30 минут
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
