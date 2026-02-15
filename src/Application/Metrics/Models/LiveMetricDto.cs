using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Metrics.Models
{
    public sealed class LiveMetricDto
    {
        public int MetricId { get; init; }
        public int ServerId { get; init; }
        public double CpuUsage { get; init; }
        public double MemoryUsage { get; init; }
        public double DiskUsage { get; init; }
        public double ResponseTime { get; init; }
        public string Status { get; init; } = null!;
        public DateTime Timestamp { get; init; }
    }
}
