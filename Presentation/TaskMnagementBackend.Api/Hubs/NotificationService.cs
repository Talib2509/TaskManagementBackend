using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Api.Hubs;
using TaskMnagementBackend.Aplication.Abstraction.Services;

namespace TaskMnagementBackend.Api.Hubs 
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationToUserAsync(string userId, object notification, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification, cancellationToken);
        }

        public async Task SendCommentToTaskGroupAsync(int taskId, object comment, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients.Group($"Task_{taskId}").SendAsync("ReceiveNewComment", comment, cancellationToken);
        }
    }
}