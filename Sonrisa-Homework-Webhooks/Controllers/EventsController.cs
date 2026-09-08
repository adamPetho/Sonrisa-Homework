using Microsoft.AspNetCore.Mvc;
using Sonrisa_Homework_Webhooks.Models.Events;
using Sonrisa_Homework_Webhooks.Parser;
using Sonrisa_Homework_Webhooks.Services;
using System.Text.Json;

namespace Sonrisa_Homework_Webhooks.Controllers
{
    [ApiController]
    [Route("api/events")]
    public sealed class EventsController : ControllerBase
    {
        private readonly IEventFactory _eventFactory;
        private readonly IAlertEngine _alertEngine;

        public EventsController(
            IEventFactory eventFactory,
            IAlertEngine alertEngine)
        {
            _eventFactory = eventFactory;
            _alertEngine = alertEngine;
        }

        [HttpPost]
        public async Task<IActionResult> Receive([FromBody] EventRequest request, CancellationToken cancellationToken)
        {
            IEvent @event;

            try
            {
                @event = _eventFactory.Create(request);
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (JsonException ex)
            {
                return BadRequest(ex.Message);
            }

            await _alertEngine.ProcessEventAsync(@event, cancellationToken);

            return Accepted();
        }
    }
}
