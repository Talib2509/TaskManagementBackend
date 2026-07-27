using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationHandler
        : IRequestHandler<DeleteNotificationRequest, DeleteNotificationResponse>
    {
        private readonly INotificationService _notificationService;

        public DeleteNotificationHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<DeleteNotificationResponse> Handle(
            DeleteNotificationRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _notificationService.DeleteNotificationAsync(
                request.NotificationId,
                request.UserId,
                cancellationToken);

            if (!result)
            {
                return new DeleteNotificationResponse
                {
                    Succeeded = false,
                    Message = "Bildiriş silinə bilmədi.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new DeleteNotificationResponse
            {
                Succeeded = true,
                Message = "Bildiriş uğurla silindi."
            };
        }
    }
}