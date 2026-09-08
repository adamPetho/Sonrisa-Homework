using Sonrisa_Homework_Webhooks.Models.Alerts;

namespace Sonrisa_Homework_Webhooks.Repositories
{
    public interface IAlertRepository
    {
        IReadOnlyCollection<IAlert> GetAll();
        IAlert? GetById(Guid id);
        void Add(IAlert alert);
        void Update(IAlert alert);
        bool Delete(Guid id);
    }
}
