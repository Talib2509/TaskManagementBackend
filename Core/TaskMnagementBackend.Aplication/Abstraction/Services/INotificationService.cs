using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface INotificationService
    {
        Task CreateAsync(Notification notification, CancellationToken cancellationToken = default);
    }
}
