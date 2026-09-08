using Sonrisa_Homework_Webhooks.Models.Events;

namespace Sonrisa_Homework_Webhooks.Repositories
{
    public interface IEventRepository
    {
        IReadOnlyCollection<IEvent> GetAll();
        IEvent? GetById(Guid id);
        void Add(IEvent alert);
        void Update(IEvent alert);
        bool Delete(Guid id);
    }
}
