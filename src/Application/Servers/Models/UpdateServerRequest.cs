using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Servers.Models
{
    public sealed class UpdateServerRequest
    {
        public string Name { get; init; } = null!;
        public string? IpAddress { get; init; }
        public string Status { get; init; } = null!;
        public string? Description { get; init; }
    }
}
