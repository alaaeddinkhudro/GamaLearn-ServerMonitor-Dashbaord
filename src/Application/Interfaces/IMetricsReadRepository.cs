using Application.Common.Models;
using Application.Metrics.Models;
using Application.Servers.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IMetricsReadRepository
    {
        Task<IReadOnlyList<ServerHourlyMetricDto>> GetServersHourlyMetricsPastHoursAsync(
               int pastHours,
               CancellationToken ct);
    }
}
