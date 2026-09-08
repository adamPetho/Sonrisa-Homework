using System.Text.Json;

namespace Sonrisa_Homework_Webhooks.Models.Events
{
    public sealed record EventRequest(string Type, JsonElement Data);
}
