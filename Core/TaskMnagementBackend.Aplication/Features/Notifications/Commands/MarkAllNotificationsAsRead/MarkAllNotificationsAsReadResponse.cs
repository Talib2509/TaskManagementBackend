using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadResponse : OperationResultBase
    {
        public int UpdatedCount { get; set; }
    }
}