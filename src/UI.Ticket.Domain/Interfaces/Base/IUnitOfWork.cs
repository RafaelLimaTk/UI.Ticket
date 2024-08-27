namespace UI.Ticket.Domain.Interfaces.Base;
public interface IUnitOfWork
{
    Task CommitAsync();
}
