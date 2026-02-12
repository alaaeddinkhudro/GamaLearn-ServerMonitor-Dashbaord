using Application.Interfaces;
using Application.Servers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Servers.Features.ListServers
{
    public sealed class ListServersService
    {
        private readonly IServerRepository _repo;

        public ListServersService(IServerRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<ServerDto>> GetAllAsync(CancellationToken ct)
        {
            var servers = await _repo.GetAllAsync(ct);

            return servers
                .Select(s => new ServerDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    IPAddress = s.IPAddress,
                    Status = s.Status,
                    Description = s.Description,
                    CreatedAt = s.CreatedAt
                })
                .ToList();
        }
    }
}
