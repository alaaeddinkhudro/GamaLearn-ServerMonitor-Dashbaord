using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Servers.Models
{
    public sealed class CreateServerRequest
    {
        public string Name { get; init; } = null!;
        public string? IpAddress { get; init; }
        public string? Status { get; init; }     // optional; default to "Up"
        public string? Description { get; init; }
    }
}
