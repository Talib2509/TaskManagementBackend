using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetNotificationById
{
    public class GetNotificationByIdHandler
        : IRequestHandler<GetNotificationByIdRequest, GetNotificationByIdResponse>
    {
        private readonly INotificationService _notificationService;

        public GetNotificationByIdHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<GetNotificationByIdResponse> Handle(
            GetNotificationByIdRequest request,
            CancellationToken cancellationToken)
        {
            var notification = await _notificationService.GetByIdAsync(
                request.NotificationId,
                request.UserId,
                cancellationToken);

            if (notification == null)
            {
                return new GetNotificationByIdResponse
                {
                    Succeeded = false,
                    Message = "Bildiriş tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetNotificationByIdResponse
            {
                Succeeded = true,
                Message = "Bildiriş uğurla əldə edildi.",
                Notification = notification
            };
        }
    }
}