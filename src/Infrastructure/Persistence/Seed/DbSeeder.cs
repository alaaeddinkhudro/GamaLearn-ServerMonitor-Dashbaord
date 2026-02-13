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
            await SeedMetricsAsync(ct);
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

        private async Task SeedMetricsAsync(CancellationToken ct)
        {
            if (await _db.Metrics.AnyAsync(ct))
                return;

            var servers = await _db.Servers
                .Select(s => s.ServerId)
                .ToListAsync(ct);

            if (!servers.Any())
                return;

            var random = new Random();
            var metrics = new List<Models.Metric>();

            foreach (var serverId in servers)
            {
                for (int i = 0; i < 100; i++)
                {
                    var timestamp = DateTime.UtcNow.AddMinutes(-i * 10);

                    var cpu = random.NextDouble() * 100;
                    var memory = random.NextDouble() * 100;
                    var disk = random.NextDouble() * 100;
                    var responseTime = random.NextDouble() * 500;

                    var status = cpu > 80 || memory > 90 || disk > 95 || responseTime > 600
                        ? "Critical"
                        : "Normal";

                    metrics.Add(new Models.Metric
                    {
                        ServerId = serverId,
                        CpuUsage = Math.Round(cpu, 2),
                        MemoryUsage = Math.Round(memory, 2),
                        DiskUsage = Math.Round(disk, 2),
                        ResponseTime = Math.Round(responseTime, 2),
                        Status = status,
                        Timestamp = timestamp
                    });
                }
            }

            _db.Metrics.AddRange(metrics);
            await _db.SaveChangesAsync(ct);
        }

    }
}
