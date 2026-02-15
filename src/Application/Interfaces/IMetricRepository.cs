using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IMetricRepository
    {
        Task CreateAsync(
            int serverId,
            double cpuUsage,
            double memoryUsage,
            double diskUsage,
            double responseTime,
            MetricStatus status,
            DateTime timestamp,
            CancellationToken ct);
    }
}
