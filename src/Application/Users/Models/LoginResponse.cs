using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Models
{
    public sealed class LoginResponse
    {
        public string AccessToken { get; init; } = null!;
        public string RefreshToken { get; init; } = null!;
    }
}
