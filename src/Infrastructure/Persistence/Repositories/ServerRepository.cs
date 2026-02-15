using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class ServerRepository : IServerRepository,  IServerExistsRepository
    {
        private readonly ApplicationDbContext _db;

        public ServerRepository(ApplicationDbContext db) => _db = db;

        public async Task<IReadOnlyList<Server>> GetAllAsync(CancellationToken ct)
        {
            var list = await _db.Servers
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);

            return list.Select(Map).ToList();
        }

        public async Task<Server?> GetByIdAsync(int id, CancellationToken ct)
        {
            var s = await _db.Servers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServerId == id, ct);

            return s is null ? null : Map(s);
        }

        public async Task<int> CreateAsync(
            string name,
            string? ipAddress,
            string status,
            string? description,
            CancellationToken ct)
        {
            var entity = new Models.Server
            {
                Name = name,
                IPAddress = ipAddress,
                Status = status,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };

            _db.Servers.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity.ServerId;
        }

        public async Task<bool> UpdateAsync(
            int id,
            string name,
            string? ipAddress,
            string status,
            string? description,
            CancellationToken ct)
        {
            var entity = await _db.Servers.FirstOrDefaultAsync(x => x.ServerId == id, ct);
            if (entity is null) return false;

            entity.Name = name;
            entity.IPAddress = ipAddress; 
            entity.Status = status;
            entity.Description = description;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _db.Servers.FirstOrDefaultAsync(x => x.ServerId == id, ct);
            if (entity is null) return false;

            _db.Servers.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct)
        {
            return await _db.Servers.AnyAsync(x => x.ServerId == id, ct);
        }

        private static Server Map(Models.Server x) => new()
        {
            Id = x.ServerId,
            Name = x.Name,
            IPAddress = x.IPAddress, 
            Status = x.Status,
            Description = x.Description,
            CreatedAt = x.CreatedAt
        };
    }
}
