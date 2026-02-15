using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IServerExistsRepository
    {
        Task<bool> ExistsAsync(int serverId, CancellationToken ct);
    }
}
