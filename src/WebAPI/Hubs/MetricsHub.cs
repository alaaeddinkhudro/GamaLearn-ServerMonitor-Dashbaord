using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace WebAPI.Hubs
{
    [Authorize]
    public sealed class MetricsHub : Hub
    {
        private readonly IServerExistsRepository _repo;

        public MetricsHub(IServerExistsRepository repo) => _repo = repo;

        // Client calls: Subscribe(1)
        public async Task Subscribe(int serverId)
        {
            // Validate server exists (prevents junk subscriptions)
            var exists = await _repo.ExistsAsync(serverId, Context.ConnectionAborted);

            if (!exists)
                throw new HubException($"Server {serverId} not found.");

            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(serverId));
        }

        // Client calls: Unsubscribe(1)
        public Task Unsubscribe(int serverId)
            => Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(serverId));

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // Groups are automatically cleaned up on disconnect
            return base.OnDisconnectedAsync(exception);
        }

        public static string GroupName(int serverId) => $"server:{serverId}";
    }
}
