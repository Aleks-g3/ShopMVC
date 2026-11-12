using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ShopMVC.Data;

public static class DatabaseInitializer
{
    private static readonly string[] Roles = { "Admin", "Member" };

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        if (EF.IsDesignTime)
            return;

        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(DatabaseInitializer));

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in Roles)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;

            var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
            if (!roleResult.Succeeded)
                throw new InvalidOperationException(
                    $"Failed to create role '{role}': {FormatErrors(roleResult)}");
        }

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var email = configuration["SeedAdmin:Email"];
        var password = configuration["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning(
                "Admin seeding skipped. Set 'SeedAdmin:Email' and 'SeedAdmin:Password' with 'dotnet user-secrets'.");
            return;
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var admin = await userManager.FindByEmailAsync(email);

        if (admin is null)
        {
            admin = new IdentityUser(email) { Email = email, EmailConfirmed = true };
            var createResult = await userManager.CreateAsync(admin, password);
            if (!createResult.Succeeded)
                throw new InvalidOperationException(
                    $"Failed to create admin user: {FormatErrors(createResult)}");
        }
        else if (!admin.EmailConfirmed)
        {
            admin.EmailConfirmed = true;
            var confirmResult = await userManager.UpdateAsync(admin);
            if (!confirmResult.Succeeded)
                throw new InvalidOperationException(
                    $"Failed to confirm admin user: {FormatErrors(confirmResult)}");
        }

        if (await userManager.IsInRoleAsync(admin, "Admin"))
            return;

        var addToRoleResult = await userManager.AddToRoleAsync(admin, "Admin");
        if (!addToRoleResult.Succeeded)
            throw new InvalidOperationException(
                $"Failed to assign admin role: {FormatErrors(addToRoleResult)}");
    }

    private static string FormatErrors(IdentityResult result) =>
        string.Join(", ", result.Errors.Select(error => error.Description));
}
