using Application.Servers.Features.ListServers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/servers")]
    public sealed class ServersController : ControllerBase
    {
        private readonly ListServersService _service;

        public ServersController(ListServersService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _service.GetAllAsync(ct);
            return Ok(result);
        }
    }
}
