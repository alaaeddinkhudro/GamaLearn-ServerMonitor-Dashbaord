using Application.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class AlertRepository : IAlertRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IAlertPublisher _alertPublisher;

        public AlertRepository(ApplicationDbContext db, IAlertPublisher alertPublisher)
        {
            _db = db;
            _alertPublisher = alertPublisher;
        }

        public async Task CreateAsync(
            int serverId,
            MetricType metricType,
            double metricValue,
            double threshold,
            AlertStatus status,
            DateTime createdAt,
            CancellationToken ct)
        {

            var alert = new Persistence.Models.Alert
            {
                ServerId = serverId,
                MetricType = metricType.ToString(),          // DB is NVARCHAR
                MetricValue = metricValue,
                Threshold = threshold,
                Status = status == AlertStatus.Triggered ? "Triggered" : "Resolved",
                CreatedAt = createdAt,
                ResolvedAt = null
            };

            _db.Alerts.Add(alert);

            await _db.SaveChangesAsync(ct);

            var _alert = new Domain.Entities.Alert
            {
                Id = alert.AlertId,
                ServerId = alert.ServerId,
                MetricType = metricType,
                MetricValue = alert.MetricValue,
                Threshold = alert.Threshold,
                Status = status,
                CreatedAt = alert.CreatedAt,
                ResolvedAt = alert.ResolvedAt
            };

            await _alertPublisher.PublishAlertAsync(_alert, ct);

        }
    }
}
