using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UI.Ticket.Domain.Accounts;

namespace UI.Ticket.Infra.Data.Identity.Services;
public class AuthenticateService : IAuthenticate
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticateService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Authentication(string email, string password, bool rememberMe)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return false;

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
            return false;

        var claims = await _userManager.GetClaimsAsync(user);
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(5)
        };

        await _httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties
        );

        return true;
    }

    public async Task Logout()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<(bool success, string msg)> RegisterUser(string fullName, string email, string password)
    {
        var applicationUser = new ApplicationUser(fullName, email);

        var result = await _userManager.CreateAsync(applicationUser, password);

        if (result.Succeeded)
        {
            await EnsureRolesExistAsync();

            await _userManager.AddToRoleAsync(applicationUser, "Support");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, applicationUser.UserName!),
                new Claim(ClaimTypes.Email, applicationUser.Email!)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties
            );
        }

        var errorMsg = result.Errors.FirstOrDefault()?.Description ?? "Erro desconhecido.";
        return (result.Succeeded, errorMsg);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<bool> UpdateUserProfile(string userId, string imgPath)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.UpdateProfilePicture(imgPath);
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        return false;
    }

    private async Task EnsureRolesExistAsync()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await _roleManager.RoleExistsAsync("Support"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Support"));
        }

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }
    }
}
