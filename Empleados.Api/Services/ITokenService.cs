using Empleados.Data.Entities;

namespace Empleados.Api.Services;

public interface ITokenService
{
    (string Token, DateTime Expiration) GenerarToken(ApplicationUser usuario, IList<string> roles);
}
