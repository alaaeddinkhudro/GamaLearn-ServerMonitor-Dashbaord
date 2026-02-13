using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Metrics.Models
{
    public sealed class ServerHourlyMetricDto
    {
        public int ServerId { get; init; }
        public DateTime HourBucket { get; init; }  // e.g. 2026-02-13 14:00:00
        public double AvgCpuUsage { get; init; }
        public double AvgMemoryUsage { get; init; }
        public double AvgDiskUsage { get; init; }
        public double AvgResponseTime { get; init; }
    }
}
