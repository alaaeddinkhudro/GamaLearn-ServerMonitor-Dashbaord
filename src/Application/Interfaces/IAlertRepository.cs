using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAlertRepository
    {
        Task CreateAsync(
            int serverId,
            MetricType metricType,
            double metricValue,
            double threshold,
            AlertStatus status,
            DateTime createdAt,
            CancellationToken ct);
    }
}
