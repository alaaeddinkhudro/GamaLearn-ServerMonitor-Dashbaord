using Application.Interfaces;
using Application.Servers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Servers.Features.ServersCrud
{
    public sealed class ServersService
    {
        private readonly IServerRepository _repo;

        public ServersService(IServerRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<ServerDto>> GetAllAsync(CancellationToken ct)
        {
            var list = await _repo.GetAllAsync(ct);
            return list.Select(ToDto).ToList();
        }

        public async Task<ServerDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var server = await _repo.GetByIdAsync(id, ct);
            return server is null ? null : ToDto(server);
        }

        public async Task<int> CreateAsync(CreateServerRequest req, CancellationToken ct)
        {
            var status = string.IsNullOrWhiteSpace(req.Status) ? "Up" : req.Status!.Trim();

            return await _repo.CreateAsync(
                name: req.Name.Trim(),
                ipAddress: string.IsNullOrWhiteSpace(req.IpAddress) ? null : req.IpAddress.Trim(),
                status: status,
                description: string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim(),
                ct: ct);
        }

        public Task<bool> UpdateAsync(int id, UpdateServerRequest req, CancellationToken ct)
            => _repo.UpdateAsync(
                id: id,
                name: req.Name.Trim(),
                ipAddress: string.IsNullOrWhiteSpace(req.IpAddress) ? null : req.IpAddress.Trim(),
                status: req.Status.Trim(),
                description: string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim(),
                ct: ct);

        public Task<bool> DeleteAsync(int id, CancellationToken ct)
            => _repo.DeleteAsync(id, ct);

        private static ServerDto ToDto(Domain.Entities.Server s) => new()
        {
            Id = s.Id,
            Name = s.Name,
            IPAddress = s.IPAddress,
            Status = s.Status,
            Description = s.Description,
            CreatedAt = s.CreatedAt
        };
    }
}
