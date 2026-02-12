using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) => _db = db;

        public async Task<(User user, string passwordHash)?> GetForLoginAsync(
            string emailOrUserName,
            CancellationToken ct)
        {
            var u = await _db.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x =>
                    x.Email == emailOrUserName ||
                    x.UserName == emailOrUserName, ct);

            if (u is null) return null;

            var user = new User
            {
                Id = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                Role = u.Role.Name
            };

            return (user, u.PasswordHash);
        }

        public async Task UpdateRefreshTokenAsync(
            int userId,
            string refreshToken,
            DateTime expiry,
            CancellationToken ct)
        {
            var user = await _db.Users.FindAsync([userId], ct);
            if (user is null) return;

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = expiry;

            await _db.SaveChangesAsync(ct);
        }
    }
}
