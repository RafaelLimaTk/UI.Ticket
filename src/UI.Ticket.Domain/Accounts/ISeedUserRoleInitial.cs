namespace UI.Ticket.Domain.Accounts;

public interface ISeedUserRoleInitial
{
    Task SeedUsersAsync();
    Task SeedRolesAsync();
}