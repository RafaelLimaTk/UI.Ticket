using Microsoft.AspNetCore.Identity;
using UI.Ticket.Domain.Accounts;

namespace UI.Ticket.Infra.Data.Identity;
public class SeedUserRoleInitial : ISeedUserRoleInitial
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedUserRoleInitial(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedRolesAsync()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            IdentityRole adminRole = new IdentityRole { Name = "Admin" };
            await _roleManager.CreateAsync(adminRole);
        }

        if (!await _roleManager.RoleExistsAsync("Support"))
        {
            IdentityRole supportRole = new IdentityRole { Name = "Support" };
            await _roleManager.CreateAsync(supportRole);
        }

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            IdentityRole userRole = new IdentityRole { Name = "User" };
            await _roleManager.CreateAsync(userRole);
        }
    }

    public async Task SeedUsersAsync()
    {
        if (await _userManager.FindByEmailAsync("admin@gmail.com") == null)
        {
            var adminUser = new ApplicationUser(
                fullName: "Administrador",
                userName: "admin@gmail.com",
                email: "admin@gmail.com",
                emailConfirmed: true,
                lockoutEnabled: false
            );

            var result = await _userManager.CreateAsync(adminUser, "@Dm1n4s3r");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        if (await _userManager.FindByEmailAsync("support@gmail.com") == null)
        {
            var supportUser = new ApplicationUser(
                fullName: "Suporte",
                userName: "support@gmail.com",
                email: "support@gmail.com",
                emailConfirmed: true,
                lockoutEnabled: false
            );

            var result = await _userManager.CreateAsync(supportUser, "@Supp0rt123");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(supportUser, "Support");
            }
        }

        if (await _userManager.FindByEmailAsync("user@gmail.com") == null)
        {
            var regularUser = new ApplicationUser(
                fullName: "Usuário",
                userName: "user@gmail.com",
                email: "user@gmail.com",
                emailConfirmed: true,
                lockoutEnabled: false
            );

            var result = await _userManager.CreateAsync(regularUser, "@Us3r456");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(regularUser, "User");
            }
        }
    }
}
