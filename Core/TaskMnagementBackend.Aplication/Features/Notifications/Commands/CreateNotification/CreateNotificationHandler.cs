using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Notification;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.CreateNotification
{
    public class CreateNotificationHandler
        : IRequestHandler<CreateNotificationRequest, CreateNotificationResponse>
    {
        private readonly INotificationService _notificationService;

        public CreateNotificationHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<CreateNotificationResponse> Handle(
            CreateNotificationRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _notificationService.CreateNotificationAsync(
                new CreateNotificationDto
                {
                    Title = request.Title,
                    Text = request.Text,
                    Type = request.Type,
                    UserId = request.UserId,
                    RelatedEntityId = request.RelatedEntityId
                },
                cancellationToken);

            if (!result)
            {
                return new CreateNotificationResponse
                {
                    Succeeded = false,
                    Message = "Bildiriş yaradıla bilmədi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new CreateNotificationResponse
            {
                Succeeded = true,
                Message = "Bildiriş uğurla yaradıldı."
            };
        }
    }
}