using Empleados.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Empleados.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedRolesAndUsersAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { Roles.Admin, Roles.RRHH })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await CreateOrRenameUserAsync(userManager, "admin@empresa.com", "admin@gmail.com", "Admin123!", "Administrador del Sistema", Roles.Admin);
        await CreateOrRenameUserAsync(userManager, "rrhh@empresa.com", "rrhh@gmail.com", "Rrhh123!", "Operador de RRHH", Roles.RRHH);
    }

    private static async Task CreateOrRenameUserAsync(
        UserManager<ApplicationUser> userManager, string oldEmail, string newEmail, string password, string nombreCompleto, string rol)
    {
        if (await userManager.FindByEmailAsync(newEmail) is not null)
        {
            return;
        }

        var usuarioExistente = await userManager.FindByEmailAsync(oldEmail);
        if (usuarioExistente is not null)
        {
            usuarioExistente.UserName = newEmail;
            usuarioExistente.Email = newEmail;
            usuarioExistente.EmailConfirmed = true;
            await userManager.UpdateAsync(usuarioExistente);
            return;
        }

        var user = new ApplicationUser
        {
            UserName = newEmail,
            Email = newEmail,
            EmailConfirmed = true,
            NombreCompleto = nombreCompleto
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, rol);
        }
    }
}
