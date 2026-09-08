using Microsoft.AspNetCore.Mvc;
using Sonrisa_Homework_Webhooks.Models.Alerts;
using Sonrisa_Homework_Webhooks.Repositories;

namespace Sonrisa_Homework_Webhooks.Controllers
{
    [ApiController]
    [Route("api/marketmovement")]
    public class MarketMovementController : ControllerBase
    {
        private readonly IAlertRepository _alertRepository;

        public MarketMovementController(IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateMarketMovementAlertRequest request)
        {
            if (request.ThresholdPercentage < 0)
                return BadRequest("Threshold percentage cannot be negative.");

            if (request.Channels.Count == 0)
                return BadRequest("At least one notification channel is required.");

            var alert = new MarketMovementAlert(
                Guid.NewGuid(),
                request.Name,
                "market.movement",
                request.Enabled,
                request.Symbol,
                request.ThresholdPercentage,
                request.Channels);

            _alertRepository.Add(alert);

            return CreatedAtAction(
                nameof(GetById),
                new { id = alert.Id },
                alert);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var alert = _alertRepository.GetById(id);

            return alert is null
                ? NotFound()
                : Ok(alert);
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Test OK.");
        }
    }

    public sealed record CreateMarketMovementAlertRequest(
        string Name,
        bool Enabled,
        string Symbol,
        decimal ThresholdPercentage,
        IReadOnlyCollection<string> Channels);
}
