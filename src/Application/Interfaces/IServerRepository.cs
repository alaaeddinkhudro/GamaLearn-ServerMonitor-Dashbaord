using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IServerRepository
    {
        Task<IReadOnlyList<Server>> GetAllAsync(CancellationToken ct);

    }
}
