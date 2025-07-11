using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Components;
using LibraryManagementSystem.Components.Account;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Utils;

namespace LibraryManagementSystem;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        builder.Services.AddScoped<AdminInitializer>();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                if (context is not ApplicationDbContext applicationDbContext)
                {
                    throw new InvalidOperationException("Context must be of type ApplicationDbContext");
                }
                var any = await applicationDbContext.Roles.AnyAsync(cancellationToken);
                if (!any)
                {
                    await applicationDbContext.Roles.AddRangeAsync(
                        new IdentityRole("admin") { NormalizedName = "ADMIN" },
                        new IdentityRole("reader") { NormalizedName = "READER" }
                    );
                }
                
                await applicationDbContext.SaveChangesAsync(cancellationToken);
            });
        });
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        
        var app = builder.Build();

        await using (var scope = app.Services.CreateAsyncScope())
        {
            await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
                .Database.EnsureCreatedAsync();
            await scope.ServiceProvider.GetRequiredService<AdminInitializer>().InitializeAdminAsync();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        app.Run();
    }
}