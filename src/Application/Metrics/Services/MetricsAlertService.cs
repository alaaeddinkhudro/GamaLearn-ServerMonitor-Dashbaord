using Application.Alerts.Features.AlertValidator;
using Application.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Metrics.Services
{

    public sealed class MetricsAlertService
    {
        private readonly AlertValidatorService _evaluator;
        private readonly IAlertRepository _alerts;

        public MetricsAlertService(AlertValidatorService evaluator, IAlertRepository alerts)
        {
            _evaluator = evaluator;
            _alerts = alerts;
        }

        public async Task EvaluateAndCreateAlertsAsync(
            int serverId,
            double cpu,
            double memory,
            double disk,
            double responseTime,
            DateTime timestampUtc,
            CancellationToken ct)
        {
            await CheckAsync(serverId, MetricType.CpuUsage, cpu, timestampUtc, ct);
            await CheckAsync(serverId, MetricType.MemoryUsage, memory, timestampUtc, ct);
            await CheckAsync(serverId, MetricType.DiskUsage, disk, timestampUtc, ct);
            await CheckAsync(serverId, MetricType.ResponseTime, responseTime, timestampUtc, ct);
        }

        private async Task CheckAsync(int serverId, MetricType type, double value, DateTime tsUtc, CancellationToken ct)
        {
            if (_evaluator.IsTriggered(type, value, out var threshold))
            {
                await _alerts.CreateAsync(
                    serverId: serverId,
                    metricType: type,
                    metricValue: value,
                    threshold: threshold,
                    status: AlertStatus.Triggered,
                    createdAt: tsUtc,
                    ct: ct);
            }
        }
    }
}
