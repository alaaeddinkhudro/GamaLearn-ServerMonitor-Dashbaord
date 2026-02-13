using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public sealed class Metric
    {
        public int Id { get; init; }
        public double CpuUsage { get; init; }
        public double MemoryUsage { get; init; }
        public double DiskUsage { get; init; }
        public double ResponseTime { get; init; }
        public string Status { get; init; } = null!;
        public DateTime Timestamp { get; init; }
    }
}
