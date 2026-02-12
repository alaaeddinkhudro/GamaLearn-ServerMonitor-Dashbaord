using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Servers.Models
{
    public sealed class ServerDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string? IPAddress { get; init; }
        public string Status { get; init; } = null!;
        public string? Description { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
