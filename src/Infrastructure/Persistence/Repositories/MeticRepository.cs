using Application.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class MetricRepository : IMetricRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMetricsPublisher _metricsPublisher;

        public MetricRepository(ApplicationDbContext db, IMetricsPublisher metricsPublisher)
        {
            _db = db;
            _metricsPublisher = metricsPublisher;
        }

        public async Task CreateAsync(
             int serverId,
            double cpuUsage,
            double memoryUsage,
            double diskUsage,
            double responseTime,
            MetricStatus status,
            DateTime timestamp,
            CancellationToken ct)
        {
            var metric = new Persistence.Models.Metric
            {
                ServerId = serverId,
                CpuUsage = cpuUsage,
                MemoryUsage = memoryUsage,
                DiskUsage = diskUsage,
                ResponseTime = responseTime,
                Status = status.ToString(),
                Timestamp = timestamp
            };
            _db.Metrics.Add(metric);

            await _db.SaveChangesAsync(ct);

            var _metric = new Domain.Entities.Metric
            {
                Id = metric.MetricId,
                ServerId = metric.ServerId,
                CpuUsage = metric.CpuUsage,
                MemoryUsage = metric.MemoryUsage,
                DiskUsage = metric.DiskUsage,
                ResponseTime = metric.ResponseTime,
                Status = status,
                Timestamp = metric.Timestamp
            };

            await _metricsPublisher.PublishMetricAsync(_metric, ct);
        }
    }
}
