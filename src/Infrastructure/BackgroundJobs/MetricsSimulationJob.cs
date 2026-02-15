using Application.Interfaces;
using Application.Metrics.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BackgroundJobs
{
    public sealed class MetricsSimulationJob
    {
        private readonly ApplicationDbContext _db;
        private readonly IMetricsPublisher _metricsPublisher;
        private static readonly Random _random = new();

        public MetricsSimulationJob(ApplicationDbContext db, IMetricsPublisher metricsPublisher)
        {
            _db = db;
            _metricsPublisher = metricsPublisher;
        }

        public async Task RunAsync(CancellationToken ct = default)
        {
            var serverIds = await _db.Servers
                .AsNoTracking()
                .Select(s => s.ServerId)
                .ToListAsync(ct);

            if (serverIds.Count == 0) return;

            var now = DateTime.UtcNow;

            List<LiveMetricDto> liveMetrics = new();

            foreach (var serverId in serverIds)
            {
                var cpu = NextDouble(5, 95);
                var mem = NextDouble(10, 98);
                var disk = NextDouble(20, 99);
                var rt = NextDouble(10, 800); // ms

                var status =
                    cpu > 80 || mem > 90 || disk > 95 || rt > 600
                        ? "Critical"
                        : "Normal";

                _db.Metrics.Add(new Persistence.Models.Metric
                {
                    ServerId = serverId,
                    CpuUsage = cpu,
                    MemoryUsage = mem,
                    DiskUsage = disk,
                    ResponseTime = rt,
                    Status = status,
                    Timestamp = now
                });

                liveMetrics.Add(new LiveMetricDto
                {
                    ServerId = serverId,
                    CpuUsage = cpu,
                    MemoryUsage = mem,
                    DiskUsage = disk,
                    ResponseTime = rt,
                    Status = status,
                    Timestamp = now
                });
            }

            await _db.SaveChangesAsync(ct);
            foreach (var metric in liveMetrics)
            {
                await _metricsPublisher.PublishMetricAsync(metric, ct);
            }
        }

        private static double NextDouble(double min, double max)
            => Math.Round(min + (_random.NextDouble() * (max - min)), 2);
    }
}
