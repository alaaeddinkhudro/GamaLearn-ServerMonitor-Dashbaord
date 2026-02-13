using Application.Interfaces;
using Application.Metrics.Models;
using Application.Servers.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class MetricsReadRepository : IMetricsReadRepository
    {
        private readonly IConfiguration _config;

        public MetricsReadRepository(IConfiguration config) => _config = config;

        public async Task<IReadOnlyList<ServerHourlyMetricDto>> GetServersHourlyMetricsPastHoursAsync(
            int pastHours,
            CancellationToken ct)
        {
            pastHours = Math.Clamp(pastHours, 1, 168); // 1h..7d safety

            const string sql = @"
                DECLARE @FromUtc DATETIME = DATEADD(hour, -@PastHours, GETUTCDATE());

                SELECT
                    m.ServerId AS ServerId,
                    DATEADD(hour, DATEDIFF(hour, 0, m.[Timestamp]), 0) AS HourBucket,
                    ROUND(AVG(m.CpuUsage), 2)      AS AvgCpuUsage,
                    ROUND(AVG(m.MemoryUsage), 2)   AS AvgMemoryUsage,
                    ROUND(AVG(m.DiskUsage), 2)     AS AvgDiskUsage,
                    ROUND(AVG(m.ResponseTime), 2)  AS AvgResponseTime
                FROM dbo.Metrics m
                WHERE m.[Timestamp] >= @FromUtc
                GROUP BY
                    m.ServerId,
                    DATEADD(hour, DATEDIFF(hour, 0, m.[Timestamp]), 0)
                ORDER BY
                    m.ServerId,
                    HourBucket;";

            var connString = _config.GetConnectionString("DefaultConnection")!;
            await using var conn = new SqlConnection(connString);
            await conn.OpenAsync(ct);

            var rows = await conn.QueryAsync<ServerHourlyMetricDto>(
                new CommandDefinition(sql, new { PastHours = pastHours }, cancellationToken: ct));

            return rows.ToList();
        }
    }
}
