using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetNotificationCount
{
    public class GetNotificationCountResponse : OperationResultBase
    {
        public int Count { get; set; }
    }
}