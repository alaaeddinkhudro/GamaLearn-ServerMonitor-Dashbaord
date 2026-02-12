using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(User user);
        string CreateRefreshToken();
    }
}
