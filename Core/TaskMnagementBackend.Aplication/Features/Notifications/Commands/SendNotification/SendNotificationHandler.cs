using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.SendNotification
{
    public class SendNotificationHandler
        : IRequestHandler<SendNotificationRequest, SendNotificationResponse>
    {
        private readonly INotificationService _notificationService;

        public SendNotificationHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<SendNotificationResponse> Handle(
            SendNotificationRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _notificationService.SendNotificationAsync(
                request.NotificationId,
                cancellationToken);

            if (!result)
            {
                return new SendNotificationResponse
                {
                    Succeeded = false,
                    Message = "Bildiriş göndərilə bilmədi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new SendNotificationResponse
            {
                Succeeded = true,
                Message = "Bildiriş uğurla göndərildi."
            };
        }
    }
}