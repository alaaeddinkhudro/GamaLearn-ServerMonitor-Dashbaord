using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IServerRepository
    {
        Task<IReadOnlyList<Server>> GetAllAsync(CancellationToken ct);
        Task<Server?> GetByIdAsync(int id, CancellationToken ct);

        Task<int> CreateAsync(
            string name,
            string? ipAddress,
            string status,
            string? description,
            CancellationToken ct);

        Task<bool> UpdateAsync(
            int id,
            string name,
            string? ipAddress,
            string status,
            string? description,
            CancellationToken ct);

        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
