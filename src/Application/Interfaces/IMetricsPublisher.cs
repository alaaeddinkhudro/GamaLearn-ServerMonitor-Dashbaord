using Application.Metrics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IMetricsPublisher
    {
        Task PublishMetricAsync(LiveMetricDto metric, CancellationToken ct = default);
    }
}
