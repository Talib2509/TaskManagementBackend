using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories.Notfication;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Persistence.Repositories.Notfication
{
    public class NotificationReadRepository : ReadRepository<Domain.Entities.Notification>, INotificationReadRepository
    {
        public NotificationReadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
