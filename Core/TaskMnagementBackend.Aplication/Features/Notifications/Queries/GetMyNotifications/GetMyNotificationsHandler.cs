using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsHandler
        : IRequestHandler<GetMyNotificationsRequest, GetMyNotificationsResponse>
    {
        private readonly INotificationService _notificationService;

        public GetMyNotificationsHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<GetMyNotificationsResponse> Handle(
            GetMyNotificationsRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _notificationService.GetUserNotificationsAsync(
                request.UserId,
                request.OnlyUnread,
                request.Page,
                request.PageSize,
                cancellationToken);

            return new GetMyNotificationsResponse
            {
                Items = result.Items,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }
    }
}