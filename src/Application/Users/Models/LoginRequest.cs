using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Models
{
    public sealed class LoginRequest
    {
        public string EmailOrUserName { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
