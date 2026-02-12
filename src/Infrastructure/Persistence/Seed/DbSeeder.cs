using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Persistence.Seed
{
    public sealed class DbSeeder
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<string> _hasher;

        public DbSeeder(ApplicationDbContext db, IPasswordHasher<string> hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        public async Task SeedAsync(CancellationToken ct)
        {
            await SeedUsersAsync(ct);
            await SeedServersAsync(ct);
        }

        private async Task SeedUsersAsync(CancellationToken ct)
        {
            if (await _db.Servers.AnyAsync(ct))
                return;

            var adminRole = await _db.Roles
                .FirstOrDefaultAsync(r => r.Name == "Admin", ct);

            if (adminRole is null)
            {
                adminRole = new Models.Role
                {
                    Name = "Admin",
                    Description = "System administrator"
                };
                _db.Roles.Add(adminRole);
                await _db.SaveChangesAsync(ct);
            }

            if (await _db.Users.AnyAsync(u => u.Email == "admin@gama.local", ct))
                return;

            var user = new Models.User
            {
                UserName = "admin",
                Email = "admin@gama.local",
                RoleId = adminRole.RoleId,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _hasher.HashPassword(
                user.UserName, "Admin@12345");

            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);
        }


        private async Task SeedServersAsync(CancellationToken ct)
        {
            if (await _db.Servers.AnyAsync(ct))
                return;

            var servers = new List<Models.Server>
            {
                new()
                {
                    Name = "Production API",
                    IPAddress = "192.168.1.10",
                    Status = "Up",
                    Description = "Main production backend server",
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name = "Staging API",
                    IPAddress = "192.168.1.20",
                    Status = "Up",
                    Description = "Staging environment",
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name = "Analytics Node",
                    IPAddress = "192.168.1.30",
                    Status = "Down",
                    Description = "Metrics processing server",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _db.Servers.AddRange(servers);
            await _db.SaveChangesAsync(ct);
        }

    }
}
