using Application.Interfaces;
using Application.Metrics.Features.MetricsRead;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/servers/metrics")]
    [Authorize]
    public sealed class MetricsController : ControllerBase
    {
        private readonly MetricsReadService _metricRead;

        public MetricsController(MetricsReadService metricsRead) => _metricRead = metricsRead;

        // GET /api/servers/metrics/hourly?pastHours=24
        [HttpGet("hourly")]
        public async Task<IActionResult> Hourly([FromQuery] int pastHours = 24, CancellationToken ct = default)
        {
            var result = await _metricRead.GetServersHourlyMetricsPastHoursAsync(pastHours, ct);
            return Ok(result);
        }
    }
}
