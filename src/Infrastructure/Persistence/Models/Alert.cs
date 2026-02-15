using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Infrastructure.Persistence.Models;

public partial class Alert
{
    public int AlertId { get; set; }

    public int ServerId { get; set; }

    public string MetricType { get; set; } = null!;

    public double MetricValue { get; set; }

    public double Threshold { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public virtual Server Server { get; set; } = null!;

    public Domain.Enums.MetricType GetMetricType() => Enum.Parse<Domain.Enums.MetricType>(MetricType);
    public void SetMetricType(Domain.Enums.MetricType metricType) => MetricType = metricType.ToString();

    public AlertStatus GetStatus() => Enum.Parse<AlertStatus>(Status);
    public void SetStatus(AlertStatus status) => Status = status.ToString();
}
