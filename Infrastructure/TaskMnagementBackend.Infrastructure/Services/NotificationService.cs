using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IWriteRepository<Notification> _notificationWriteRepository;

        public NotificationService(IWriteRepository<Notification> notificationWriteRepository)
        {
            _notificationWriteRepository = notificationWriteRepository;
        }

        public async Task CreateAsync(Notification notification, CancellationToken cancellationToken = default)
        {
            await _notificationWriteRepository.AddAsync(notification);
            await _notificationWriteRepository.SaveAsync();     
        }
    }
}
