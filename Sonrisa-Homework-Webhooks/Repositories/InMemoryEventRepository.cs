using Sonrisa_Homework_Webhooks.Models.Events;
using System.Collections.Concurrent;

namespace Sonrisa_Homework_Webhooks.Repositories
{
    public class InMemoryEventRepository : IEventRepository
    {
        private readonly ConcurrentDictionary<Guid, IEvent> Events = new();
        public void Add(IEvent alert)
        {
            if (!Events.TryAdd(alert.Id, alert))
            {
                throw new InvalidOperationException(
                    $"An alert with ID '{alert.Id}' already exists.");
            }
        }

        public bool Delete(Guid id)
        {
            return Events.TryRemove(id, out _);
        }

        public IReadOnlyCollection<IEvent> GetAll()
        {
            return Events.Values.ToArray();
        }

        public IEvent? GetById(Guid id)
        {
            return Events.TryGetValue(id, out var alert)
                ? alert
                : null;
        }

        public void Update(IEvent alert)
        {
            if (!Events.TryGetValue(alert.Id, out _))
            {
                throw new KeyNotFoundException(
                    $"Alert with ID '{alert.Id}' was not found.");
            }

            Events[alert.Id] = alert;
        }
    }
}
