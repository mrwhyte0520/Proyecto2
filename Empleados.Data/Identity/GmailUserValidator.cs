using System.Text.RegularExpressions;
using Empleados.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Empleados.Data.Identity;

public class GmailUserValidator : IUserValidator<ApplicationUser>
{
    private static readonly Regex GmailRegex = new(@"^[^@\s]+@gmail\.com$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        if (string.IsNullOrWhiteSpace(user.Email) || !GmailRegex.IsMatch(user.Email))
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "EmailDebeSerGmail",
                Description = "El correo del usuario debe ser una cuenta de Gmail (@gmail.com)."
            }));
        }

        return Task.FromResult(IdentityResult.Success);
    }
}
