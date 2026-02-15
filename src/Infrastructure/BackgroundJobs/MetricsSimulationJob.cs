using Application.Alerts.Features.AlertValidator;
using Application.Interfaces;
using Application.Metrics.Models;
using Application.Metrics.Services;
using Domain.Entities;
using Domain.Enums;
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
        private readonly AlertValidatorService _alertValidatorService;
        private readonly IMetricRepository _metricRepository;
        private readonly MetricsAlertService _metricsAlertService;
        private static readonly Random _random = new();

        public MetricsSimulationJob(ApplicationDbContext db,
            AlertValidatorService alertValidatorService,
            IMetricsPublisher metricsPublisher,
            IMetricRepository metricRepository,
            MetricsAlertService metricsAlertService)
        {
            _db = db;
            _alertValidatorService = alertValidatorService;
            _metricRepository = metricRepository;
            _metricsAlertService = metricsAlertService;
        }

        public async Task RunAsync(CancellationToken ct = default)
        {
            var serverIds = await _db.Servers
                .AsNoTracking()
                .Select(s => s.ServerId)
                .ToListAsync(ct);

            if (serverIds.Count == 0) return;

            var now = DateTime.UtcNow;

            List<Metric> liveMetrics = new();

            foreach (var serverId in serverIds)
            {
                var cpu = NextDouble(5, 95);
                var mem = NextDouble(10, 98);
                var disk = NextDouble(20, 99);
                var rt = NextDouble(10, 800); // ms

                var isCritical = _alertValidatorService.IsTriggered(MetricType.CpuUsage, cpu, out _)
                    || _alertValidatorService.IsTriggered(MetricType.MemoryUsage, mem, out _)
                    || _alertValidatorService.IsTriggered(MetricType.DiskUsage, disk, out _)
                    || _alertValidatorService.IsTriggered(MetricType.ResponseTime, rt, out _);

                var status = isCritical ? MetricStatus.Critical : MetricStatus.Normal;

                await _metricRepository.CreateAsync(
                    serverId,
                    cpu,
                    mem,
                    disk,
                    rt,
                    status,
                    now,
                    ct);

                await _metricsAlertService.EvaluateAndCreateAlertsAsync(
                    serverId: serverId,
                    cpu: cpu,
                    memory: mem,
                    disk: disk,
                    responseTime: rt,
                    timestampUtc: now,
                    ct: CancellationToken.None);

               
            }
        }

        private static double NextDouble(double min, double max)
            => Math.Round(min + (_random.NextDouble() * (max - min)), 2);
    }
}
