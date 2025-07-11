using LibraryManagementSystem.Components.Account.Pages;
using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Utils;

public class AdminInitializer
{
    private ApplicationDbContext _applicationDbContext;
    private UserManager<ApplicationUser> _userManager;
    private IUserStore<ApplicationUser> _userStore;
    //private RoleManager<IdentityRole> _roleManager;
    private ILogger<Register> _logger;
    
    public AdminInitializer(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, ILogger<Register> logger, ApplicationDbContext applicationDbContext)
    {
        _userManager = userManager;
        _userStore = userStore;
        _logger = logger;
        _applicationDbContext = applicationDbContext;
       // _roleManager = roleManager;
    }

    public async Task InitializeAdminAsync()
    {
        if (_applicationDbContext.Users.Any()) return;
        const string emailAddress = "admin@admin.com";
        const string password = "Admin@123";
        var user = CreateUser();
        
        await _userStore.SetUserNameAsync(user, emailAddress, CancellationToken.None);
        var emailStore = GetEmailStore();
        await emailStore.SetEmailAsync(user, emailAddress, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, password);
        
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(
                Environment.NewLine, result.Errors.Select(e => e.Description)));
        }
        
        user.EmailConfirmed = true;
        
        var adminRole = await _applicationDbContext.Roles.SingleOrDefaultAsync(r => r.Name == "admin");
        if (adminRole == null)
        {
            throw new InvalidOperationException("Admin role not found");
        }
        _applicationDbContext.UserRoles.Add(new IdentityUserRole<string> { RoleId = adminRole.Id, UserId = user.Id });
        
        _logger.LogInformation("Auto created a init admin account. {EmailAddress}, {Password}", emailAddress, password);
        await _applicationDbContext.SaveChangesAsync();
    }
    
    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                                                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
        }
    }
    
    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
}