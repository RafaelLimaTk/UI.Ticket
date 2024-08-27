using UI.Ticket.Domain.Entities.Base;

namespace UI.Ticket.Domain.Interfaces.Base;
public interface IRepository<T> where T : IEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task SaveAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
