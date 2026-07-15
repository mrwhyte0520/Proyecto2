using Microsoft.AspNetCore.Identity;

namespace Empleados.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string NombreCompleto { get; set; } = string.Empty;
}
