using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public sealed record ThresholdRule(MetricType Type, double Threshold);
}
