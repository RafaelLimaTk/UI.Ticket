using Microsoft.EntityFrameworkCore;
using UI.Ticket.Domain.Entities.Base;
using UI.Ticket.Domain.Interfaces.Base;
using UI.Ticket.Infra.Data.Context;

namespace UI.Ticket.Infra.Data.Repositories.Base;
public class Repository<T>
    : IRepository<T> where T : class, IEntity
{
    private readonly ApplicationDbContext _context;
    public DbSet<T> Entities { get; }

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        Entities = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await Entities.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching all entities", ex);
        }
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await Entities.FindAsync(id);
            if (entity is null)
                throw new KeyNotFoundException($"Entity with id {id} not found");

            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching entity with id {id}", ex);
        }
    }

    public async Task SaveAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            await Entities.AddAsync(entity);
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving entity", ex);
        }
    }

    public void Update(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            _context.Update(entity);
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating entity", ex);
        }
    }

    public void Delete(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        try
        {
            Entities.Remove(entity);
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting entity", ex);
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
