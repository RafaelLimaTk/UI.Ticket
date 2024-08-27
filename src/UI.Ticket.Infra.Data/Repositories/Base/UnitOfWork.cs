using UI.Ticket.Domain.Interfaces.Base;
using UI.Ticket.Infra.Data.Context;

namespace UI.Ticket.Infra.Data.Repositories.Base;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
        => _context = context;

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error committing transaction", ex);
        }
    }
}
