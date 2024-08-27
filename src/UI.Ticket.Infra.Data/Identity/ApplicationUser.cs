using Microsoft.AspNetCore.Identity;

namespace UI.Ticket.Infra.Data.Identity;
public class ApplicationUser : IdentityUser
{
    public string? FullName { get; private set; }
    public string? ProfilePicture { get; private set; }

    protected ApplicationUser() { }

    public ApplicationUser(
        string fullName,
        string userName,
        string email,
        bool emailConfirmed,
        bool lockoutEnabled,
        string? profilePicture = null
    )
    {
        FullName = fullName;
        UserName = userName;
        Email = email;
        NormalizedUserName = userName.ToUpper();
        NormalizedEmail = email.ToUpper();
        EmailConfirmed = emailConfirmed;
        LockoutEnabled = lockoutEnabled;
        SecurityStamp = Guid.NewGuid().ToString();
        ProfilePicture = profilePicture;
    }

    public ApplicationUser(string fullName, string email)
    {
        FullName = fullName;
        UserName = email;
        Email = email;
        NormalizedUserName = email.ToUpper();
        NormalizedEmail = email.ToUpper();
        SecurityStamp = Guid.NewGuid().ToString();
    }

    public void UpdateProfilePicture(string profilePicture)
        => ProfilePicture = profilePicture;

    public void UpdateFullName(string fullName)
        => FullName = fullName;
}
