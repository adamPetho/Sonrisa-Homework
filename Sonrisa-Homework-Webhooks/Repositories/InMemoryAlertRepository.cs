using Sonrisa_Homework_Webhooks.Models.Alerts;
using System.Collections.Concurrent;

namespace Sonrisa_Homework_Webhooks.Repositories
{
    public class InMemoryAlertRepository : IAlertRepository
    {
        private readonly ConcurrentDictionary<Guid, IAlert> Alerts = new();
        public void Add(IAlert alert)
        {
            if (!Alerts.TryAdd(alert.Id, alert))
            {
                throw new InvalidOperationException(
                    $"An alert with ID '{alert.Id}' already exists.");
            }
        }

        public void Delete(Guid id)
        {
            Alerts.TryRemove(id, out _);
        }

        public IReadOnlyCollection<IAlert> GetAll()
        {
            return Alerts.Values.ToArray();
        }

        public IAlert? GetById(Guid id)
        {
            return Alerts.TryGetValue(id, out var alert)
                ? alert
                : null;
        }

        public void Update(IAlert alert)
        {
            if (!Alerts.TryGetValue(alert.Id, out _))
            {
                throw new KeyNotFoundException(
                    $"Alert with ID '{alert.Id}' was not found.");
            }

            Alerts[alert.Id] = alert;
        }
    }
}
