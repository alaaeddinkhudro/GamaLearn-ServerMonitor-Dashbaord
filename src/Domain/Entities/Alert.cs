using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public sealed class Alert
    {

        public int Id { get; set; }

        public int ServerId { get; set; }

        public MetricType MetricType { get; set; }

        public double MetricValue { get; set; }

        public double Threshold { get; set; }

        public AlertStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }
    }
}
