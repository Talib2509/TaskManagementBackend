using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadHandler
        : IRequestHandler<MarkAllNotificationsAsReadRequest, MarkAllNotificationsAsReadResponse>
    {
        private readonly INotificationService _notificationService;

        public MarkAllNotificationsAsReadHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<MarkAllNotificationsAsReadResponse> Handle(
            MarkAllNotificationsAsReadRequest request,
            CancellationToken cancellationToken)
        {
            var updatedCount = await _notificationService.MarkAllAsReadAsync(
                request.UserId,
                cancellationToken);

            return new MarkAllNotificationsAsReadResponse
            {
                Succeeded = true,
                UpdatedCount = updatedCount,
                Message = $"{updatedCount} bildiriş oxunmuş kimi işarələndi."
            };
        }
    }
}