using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetNotificationCount
{
    public class GetNotificationCountHandler
        : IRequestHandler<GetNotificationCountRequest, GetNotificationCountResponse>
    {
        private readonly INotificationService _notificationService;

        public GetNotificationCountHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<GetNotificationCountResponse> Handle(
            GetNotificationCountRequest request,
            CancellationToken cancellationToken)
        {
            var count = await _notificationService.GetNotificationCountAsync(
                request.UserId,
                request.OnlyUnread,
                cancellationToken);

            return new GetNotificationCountResponse
            {
                Succeeded = true,
                Message = "Bildiriş sayı uğurla əldə edildi.",
                Count = count
            };
        }
    }
}