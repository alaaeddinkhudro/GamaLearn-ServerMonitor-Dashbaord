using Application.Servers.Features.ServersCrud;
using Application.Servers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/servers")]
    public sealed class ServersController : ControllerBase
    {
        private readonly ServersService _service;

        public ServersController(ServersService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => Ok(await _service.GetAllAsync(ct));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
        {
            var server = await _service.GetByIdAsync(id, ct);
            return server is null ? NotFound() : Ok(server);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateServerRequest req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Name is required.");

            var id = await _service.CreateAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateServerRequest req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Status))
                return BadRequest("Name and Status are required.");

            var updated = await _service.UpdateAsync(id, req, ct);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id, ct);
            return deleted ? NoContent() : NotFound();
        }
    }
}
