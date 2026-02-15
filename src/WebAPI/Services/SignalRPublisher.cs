using Application.Interfaces;
using Application.Metrics.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Hubs;
using Domain.Entities;

namespace WebAPI.Services
{
    public sealed class SignalRPublisher : IMetricsPublisher, IAlertPublisher
    {
        private readonly IHubContext<MetricsHub> _hub;

        public SignalRPublisher(IHubContext<MetricsHub> hub) => _hub = hub;

        public Task PublishMetricAsync(Metric metric, CancellationToken ct = default)
        {
            // event name: "metricAdded"
            return _hub.Clients
                .Group(MetricsHub.GroupName(metric.ServerId))
                .SendAsync("metricAdded", metric, ct);
        }

        public Task PublishAlertAsync(Alert alert, CancellationToken ct = default)
        {
            // event name: "alertRaised"
            return _hub.Clients
                .Group(MetricsHub.GroupName(alert.ServerId))
                .SendAsync("alertRaised", alert, ct);
        }
    }
}
