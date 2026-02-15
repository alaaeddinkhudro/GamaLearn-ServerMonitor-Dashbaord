using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public sealed class Metric
    { 
        public int Id { get; init; }
        public int ServerId { get; init; }
        public double CpuUsage { get; init; }
        public double MemoryUsage { get; init; }
        public double DiskUsage { get; init; }
        public double ResponseTime { get; init; }
        public MetricStatus Status { get; init; }
        public DateTime Timestamp { get; init; }
    }
}
