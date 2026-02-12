using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public sealed class User
    {
        public int Id { get; init; }
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Role { get; init; } = null!;
    }
}
