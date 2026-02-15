using Application.Interfaces;
using Application.Servers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Servers.Features.ServerExistance
{
    public sealed class ServerExistanceService
    {
        private readonly IServerExistsRepository _repo;

        public ServerExistanceService(IServerExistsRepository repo) => _repo = repo;

        public Task<bool> ExistsAsync(int serverId, CancellationToken ct)
            => _repo.ExistsAsync(serverId, ct);
    }
}
