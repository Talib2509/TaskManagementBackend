using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetUnreadNotifications
{
    public class GetUnreadNotificationsRequest
        : IRequest<GetUnreadNotificationsResponse>
    {
        public Guid UserId { get; set; }
    }
}