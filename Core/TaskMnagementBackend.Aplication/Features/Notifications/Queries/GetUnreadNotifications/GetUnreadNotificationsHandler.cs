using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetUnreadNotifications
{
    public class GetUnreadNotificationsHandler
        : IRequestHandler<GetUnreadNotificationsRequest, GetUnreadNotificationsResponse>
    {
        private readonly INotificationService _notificationService;

        public GetUnreadNotificationsHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<GetUnreadNotificationsResponse> Handle(
            GetUnreadNotificationsRequest request,
            CancellationToken cancellationToken)
        {
            var notifications = await _notificationService.GetUnreadNotificationsAsync(
                request.UserId,
                cancellationToken);

            return new GetUnreadNotificationsResponse
            {
                Succeeded = true,
                Message = "Oxunmamış bildirişlər uğurla əldə edildi.",
                Notifications = notifications
            };
        }
    }
}