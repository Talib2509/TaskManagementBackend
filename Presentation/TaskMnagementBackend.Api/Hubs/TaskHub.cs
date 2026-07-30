using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Api.Hubs
{
    public class TaskHub : Hub
    {
        
        public async Task JoinTaskGroup(int taskId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Task_{taskId}");
        }

        public async Task LeaveTaskGroup(int taskId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Task_{taskId}");
        }

        
        public async Task SendTypingStatus(int taskId, string userName, bool isTyping)
        {
            await Clients.OthersInGroup($"Task_{taskId}")
                .SendAsync("UserTypingStatus", userName, isTyping);
        }
    }
}