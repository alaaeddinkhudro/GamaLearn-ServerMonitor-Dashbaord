using Application.Interfaces;
using Application.Metrics.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Hubs;

namespace WebAPI.Services
{
    public sealed class MetricsPublisher : IMetricsPublisher
    {
        private readonly IHubContext<MetricsHub> _hub;

        public MetricsPublisher(IHubContext<MetricsHub> hub) => _hub = hub;

        public Task PublishMetricAsync(LiveMetricDto metric, CancellationToken ct = default)
        {
            // event name: "metricAdded"
            return _hub.Clients
                .Group(MetricsHub.GroupName(metric.ServerId))
                .SendAsync("metricAdded", metric, ct);
        }
    }
}
