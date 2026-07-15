using Empleados.Api.DTOs;
using Empleados.Api.Services;
using Empleados.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Empleados.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    /// <summary>Autentica un usuario y devuelve un token JWT.</summary>
    /// <response code="200">Login exitoso, devuelve el token.</response>
    /// <response code="401">Credenciales inválidas.</response>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponseDto>> Login(LoginDto dto)
    {
        var usuario = await _userManager.FindByEmailAsync(dto.Email);
        if (usuario is null || !await _userManager.CheckPasswordAsync(usuario, dto.Password))
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Credenciales inválidas"
            });
        }

        var roles = await _userManager.GetRolesAsync(usuario);
        var (token, expiration) = _tokenService.GenerarToken(usuario, roles);

        return Ok(new TokenResponseDto
        {
            Token = token,
            Expiration = expiration,
            Email = usuario.Email ?? string.Empty,
            Roles = roles.ToList()
        });
    }
}
