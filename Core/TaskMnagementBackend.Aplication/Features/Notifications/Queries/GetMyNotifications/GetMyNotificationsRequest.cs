using MediatR;
using TaskMnagementBackend.Aplication.Common.Pagination;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsRequest
        : PagedRequest, IRequest<GetMyNotificationsResponse>
    {
        public Guid UserId { get; set; }

        public bool OnlyUnread { get; set; } = false;
    }
}