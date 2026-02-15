using Domain.Enums;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Metrics.Thresholds
{
    public static class DefaultThresholdRules
    {
        public static readonly ThresholdRule[] Rules =
            [
                new ThresholdRule(MetricType.CpuUsage, 85),
                new ThresholdRule(MetricType.MemoryUsage, 90),
                new ThresholdRule(MetricType.DiskUsage, 95),
                new ThresholdRule(MetricType.ResponseTime, 600),
            ];
    }
}
