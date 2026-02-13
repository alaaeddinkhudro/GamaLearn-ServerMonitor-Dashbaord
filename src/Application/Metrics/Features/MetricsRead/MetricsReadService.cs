using Application.Interfaces;
using Application.Metrics.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Metrics.Features.MetricsRead
{
    public sealed class MetricsReadService
    {
        private readonly IMetricsReadRepository _readRepo;

        public MetricsReadService(IMetricsReadRepository readRepo) => _readRepo = readRepo;

        public Task<IReadOnlyList<ServerHourlyMetricDto>> GetServersHourlyMetricsPastHoursAsync(
               int pastHours,
               CancellationToken ct) => _readRepo.GetServersHourlyMetricsPastHoursAsync(pastHours, ct);
    }
}
