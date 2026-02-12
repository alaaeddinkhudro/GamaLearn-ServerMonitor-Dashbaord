using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class ServerRepository : IServerRepository
    {
        private readonly ApplicationDbContext _db;

        public ServerRepository(ApplicationDbContext db) => _db = db;

        public async Task<IReadOnlyList<Server>> GetAllAsync(CancellationToken ct)
        {
            // This assumes scaffolded entity is _db.Servers with properties:
            // ServerId, Name, Ipaddress, Status, Description, CreatedAt
            var list = await _db.Servers
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);

            return list.Select(x => new Server
            {
                Id = x.ServerId,
                Name = x.Name,
                IPAddress = x.IPAddress, 
                Status = x.Status,
                Description = x.Description,
                CreatedAt = x.CreatedAt
            }).ToList();
        }
    }
}
