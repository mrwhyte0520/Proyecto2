using Empleados.Api.DTOs;
using Empleados.Data.Entities;
using Empleados.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Empleados.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/empleados")]
public class EmpleadosController : ControllerBase
{
    private const long TamanoMaximoFotoBytes = 5 * 1024 * 1024;
    private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png" };

    private readonly IEmpleadoService _empleadoService;
    private readonly IWebHostEnvironment _environment;

    public EmpleadosController(IEmpleadoService empleadoService, IWebHostEnvironment environment)
    {
        _empleadoService = empleadoService;
        _environment = environment;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<EmpleadoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<EmpleadoDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nombre = null,
        [FromQuery] string? apellido = null,
        [FromQuery] DateTime? fechaContratacion = null)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        var (items, totalCount) = await _empleadoService.GetPagedAsync(page, pageSize, nombre, apellido, fechaContratacion);

        return Ok(new PagedResultDto<EmpleadoDto>
        {
            Items = items.Select(ToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmpleadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmpleadoDto>> GetById(int id)
    {
        var empleado = await _empleadoService.GetByIdAsync(id);
        return Ok(ToDto(empleado));
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ProducesResponseType(typeof(EmpleadoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmpleadoDto>> Create(EmpleadoCreateDto dto)
    {
        var creado = await _empleadoService.CreateAsync(ToEntity(dto));
        creado = await _empleadoService.GetByIdAsync(creado.Id);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, ToDto(creado));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ProducesResponseType(typeof(EmpleadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmpleadoDto>> Update(int id, EmpleadoUpdateDto dto)
    {
        var actualizado = await _empleadoService.UpdateAsync(id, ToEntity(dto));
        actualizado = await _empleadoService.GetByIdAsync(actualizado.Id);
        return Ok(ToDto(actualizado));
    }

    [HttpPatch("{id:int}/desactivar")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Desactivar(int id)
    {
        await _empleadoService.DesactivarAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:int}/reactivar")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Reactivar(int id)
    {
        await _empleadoService.ReactivarAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:int}/despedir")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Despedir(int id, [FromBody] DespedirDto? dto)
    {
        await _empleadoService.DespedirAsync(id, dto?.FechaTerminacion);
        return NoContent();
    }

    [HttpPost("{id:int}/foto")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ProducesResponseType(typeof(EmpleadoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmpleadoDto>> SubirFoto(int id, IFormFile foto)
    {
        if (foto.Length == 0)
        {
            return BadRequest(new ProblemDetails { Title = "Debe adjuntar un archivo." });
        }

        if (foto.Length > TamanoMaximoFotoBytes)
        {
            return BadRequest(new ProblemDetails { Title = "El archivo no puede superar los 5 MB." });
        }

        var extension = Path.GetExtension(foto.FileName).ToLowerInvariant();
        if (!ExtensionesPermitidas.Contains(extension))
        {
            return BadRequest(new ProblemDetails { Title = "Solo se permiten imágenes .jpg, .jpeg o .png." });
        }

        await _empleadoService.GetByIdAsync(id);

        var carpetaDestino = Path.Combine(_environment.WebRootPath, "uploads", "empleados");
        Directory.CreateDirectory(carpetaDestino);

        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);

        await using (var stream = new FileStream(rutaFisica, FileMode.Create))
        {
            await foto.CopyToAsync(stream);
        }

        var rutaRelativa = $"/uploads/empleados/{nombreArchivo}";
        var empleado = await _empleadoService.SetFotografiaAsync(id, rutaRelativa);

        return Ok(ToDto(empleado));
    }

    private static EmpleadoDto ToDto(Empleado empleado)
    {
        return new EmpleadoDto
        {
            Id = empleado.Id,
            FotografiaRuta = empleado.FotografiaRuta,
            Nombre = empleado.Nombre,
            Apellido = empleado.Apellido,
            DepartamentoId = empleado.DepartamentoId,
            DepartamentoNombre = empleado.Departamento?.Nombre ?? string.Empty,
            Puesto = empleado.Puesto,
            Salario = empleado.Salario,
            FechaNacimiento = empleado.FechaNacimiento,
            FechaContratacion = empleado.FechaContratacion,
            FechaTerminacion = empleado.FechaTerminacion,
            Direccion = empleado.Direccion,
            Telefono = empleado.Telefono,
            Email = empleado.Email,
            Estado = empleado.Estado.ToString()
        };
    }

    private static Empleado ToEntity(EmpleadoCreateDto dto)
    {
        return new Empleado
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            DepartamentoId = dto.DepartamentoId,
            Puesto = dto.Puesto,
            Salario = dto.Salario,
            FechaNacimiento = dto.FechaNacimiento,
            FechaContratacion = dto.FechaContratacion,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            Email = dto.Email
        };
    }
}
