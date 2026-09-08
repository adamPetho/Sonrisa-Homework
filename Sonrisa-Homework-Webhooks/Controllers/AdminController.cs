using Microsoft.AspNetCore.Mvc;
using Sonrisa_Homework_Webhooks.Models.Alerts;
using Sonrisa_Homework_Webhooks.Models.Events;
using Sonrisa_Homework_Webhooks.Repositories;

namespace Sonrisa_Homework_Webhooks.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public sealed class AdminController : ControllerBase
    {
        private const string AdminKeyHeader = "X-Admin-Key";

        private readonly IAlertRepository _alertRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IConfiguration _configuration;

        public AdminController(
            IAlertRepository alertRepository,
            IEventRepository eventRepository,
            IConfiguration configuration)
        {
            _alertRepository = alertRepository;
            _eventRepository = eventRepository;
            _configuration = configuration;
        }

        // --------------------
        // Alerts
        // --------------------

        [HttpGet("alerts")]
        public IActionResult GetAlerts()
        {
            if (!IsAdmin())
                return Unauthorized();

            return Ok(_alertRepository.GetAll());
        }

        [HttpGet("alerts/{id:guid}")]
        public IActionResult GetAlert(Guid id)
        {
            if (!IsAdmin())
                return Unauthorized();

            var alert = _alertRepository.GetById(id);

            return alert is null
                ? NotFound()
                : Ok(alert);
        }

        [HttpPost("alerts")]
        public IActionResult CreateAlert([FromBody] IAlert alert)
        {
            if (!IsAdmin())
                return Unauthorized();

            _alertRepository.Add(alert);

            return CreatedAtAction(
                nameof(GetAlert),
                new { id = alert.Id },
                alert);
        }

        [HttpPut("alerts/{id:guid}")]
        public IActionResult UpdateAlert(
            Guid id,
            [FromBody] IAlert alert)
        {
            if (!IsAdmin())
                return Unauthorized();

            if (id != alert.Id)
                return BadRequest("Route ID does not match alert ID.");

            try
            {
                _alertRepository.Update(alert);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return Ok(alert);
        }

        [HttpDelete("alerts/{id:guid}")]
        public IActionResult DeleteAlert(Guid id)
        {
            if (!IsAdmin())
                return Unauthorized();

            return _alertRepository.Delete(id)
                ? NoContent()
                : NotFound();
        }

        // --------------------
        // Events
        // --------------------

        [HttpGet("events")]
        public IActionResult GetEvents()
        {
            if (!IsAdmin())
                return Unauthorized();

            return Ok(_eventRepository.GetAll());
        }

        [HttpGet("events/{id:guid}")]
        public IActionResult GetEvent(Guid id)
        {
            if (!IsAdmin())
                return Unauthorized();

            var @event = _eventRepository.GetById(id);

            return @event is null
                ? NotFound()
                : Ok(@event);
        }

        [HttpPost("events")]
        public IActionResult CreateEvent([FromBody] IEvent @event)
        {
            if (!IsAdmin())
                return Unauthorized();

            _eventRepository.Add(@event);

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = @event.Id },
                @event);
        }

        [HttpDelete("events/{id:guid}")]
        public IActionResult DeleteEvent(Guid id)
        {
            if (!IsAdmin())
                return Unauthorized();

            return _eventRepository.Delete(id)
                ? NoContent()
                : NotFound();
        }

        // --------------------
        // Authentication
        // --------------------

        private bool IsAdmin()
        {
            if (!Request.Headers.TryGetValue(
                    AdminKeyHeader,
                    out var providedKey))
            {
                return false;
            }

            var configuredKeys = _configuration["AdminKeys"];

            if (string.IsNullOrWhiteSpace(configuredKeys))
                return false;

            return configuredKeys
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Contains(
                    providedKey.ToString(),
                    StringComparer.Ordinal);
        }
    }
}
