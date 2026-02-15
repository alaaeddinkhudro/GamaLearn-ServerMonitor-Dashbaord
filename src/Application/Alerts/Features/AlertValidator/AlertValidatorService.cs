using Application.Interfaces;
using Application.Metrics.Models;
using Application.Metrics.Thresholds;
using Domain.Enums;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Alerts.Features.AlertValidator
{
    public sealed class AlertValidatorService
    {
        private readonly IAlertPublisher _publisher;
        private readonly IReadOnlyDictionary<MetricType, double> _thresholds;
        public AlertValidatorService(IAlertPublisher publisher) { 
            _publisher = publisher;
            _thresholds = DefaultThresholdRules.Rules.ToDictionary(r => r.Type, r => r.Threshold);

        }

        public bool TryGetThreshold(MetricType type, out double threshold)
            => _thresholds.TryGetValue(type, out threshold);

        public bool IsTriggered(MetricType type, double value, out double threshold)
        {
            threshold = _thresholds[type];
            return value >= threshold;
        }

    }
}
