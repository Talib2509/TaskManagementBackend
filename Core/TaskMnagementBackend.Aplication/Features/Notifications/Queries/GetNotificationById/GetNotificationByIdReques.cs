using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetNotificationById
{
    public class GetNotificationByIdRequest
        : IRequest<GetNotificationByIdResponse>
    {
        public int NotificationId { get; set; }

        public Guid UserId { get; set; }
    }
}