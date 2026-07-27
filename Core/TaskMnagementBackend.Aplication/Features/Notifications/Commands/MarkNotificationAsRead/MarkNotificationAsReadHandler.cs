using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadHandler
        : IRequestHandler<MarkNotificationAsReadRequest, MarkNotificationAsReadResponse>
    {
        private readonly INotificationService _notificationService;

        public MarkNotificationAsReadHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<MarkNotificationAsReadResponse> Handle(
            MarkNotificationAsReadRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _notificationService.MarkAsReadAsync(
                request.NotificationId,
                request.UserId,
                cancellationToken);

            if (!result)
            {
                return new MarkNotificationAsReadResponse
                {
                    Succeeded = false,
                    Message = "Bildiriş oxunmuş kimi işarələnə bilmədi.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new MarkNotificationAsReadResponse
            {
                Succeeded = true,
                Message = "Bildiriş oxunmuş kimi işarələndi."
            };
        }
    }
}