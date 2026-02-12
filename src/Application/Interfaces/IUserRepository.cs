using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<(User user, string passwordHash)?> GetForLoginAsync(
            string emailOrUserName,
            CancellationToken ct);

        Task UpdateRefreshTokenAsync(
            int userId,
            string refreshToken,
            DateTime expiry,
            CancellationToken ct);
    }
}
