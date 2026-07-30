using System.Threading;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface INotificationService
    {
        Task SendNotificationToUserAsync(string userId, object notification, CancellationToken cancellationToken = default);
        Task SendCommentToTaskGroupAsync(int taskId, object comment, CancellationToken cancellationToken = default);
    }
}