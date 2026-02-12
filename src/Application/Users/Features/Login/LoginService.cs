using Application.Interfaces;
using Application.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace Application.Users.Features.Login
{
    public sealed class LoginService
    {
        private readonly IUserRepository _users;
        private readonly IJwtTokenService _jwt;
        private readonly IPasswordHasher<string> _hasher;

        public LoginService(
            IUserRepository users,
            IJwtTokenService jwt,
            IPasswordHasher<string> hasher)
        {
            _users = users;
            _jwt = jwt;
            _hasher = hasher;
        }

        public async Task<LoginResponse?> LoginAsync(
            LoginRequest request,
            CancellationToken ct)
        {
            var auth = await _users.GetForLoginAsync(
                request.EmailOrUserName.Trim(), ct);

            if (auth is null) return null;

            var (user, hash) = auth.Value;

            var result = _hasher.VerifyHashedPassword(
                user.UserName, hash, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            var refreshToken = _jwt.CreateRefreshToken();

            await _users.UpdateRefreshTokenAsync(
                user.Id,
                refreshToken,
                DateTime.UtcNow.AddDays(7),
                ct);

            return new LoginResponse
            {
                AccessToken = _jwt.CreateAccessToken(user),
                RefreshToken = refreshToken
            };
        }
    }
}
