using Empleados.Api.DTOs;
using Empleados.Data.Entities;
using Empleados.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Empleados.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/departamentos")]
public class DepartamentosController : ControllerBase
{
    private readonly IDepartamentoService _departamentoService;

    public DepartamentosController(IDepartamentoService departamentoService)
    {
        _departamentoService = departamentoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<DepartamentoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DepartamentoDto>>> GetAll()
    {
        var departamentos = await _departamentoService.GetAllAsync();
        return Ok(departamentos.Select(ToDto).ToList());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DepartamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartamentoDto>> GetById(int id)
    {
        var departamento = await _departamentoService.GetByIdAsync(id);
        return Ok(ToDto(departamento));
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ProducesResponseType(typeof(DepartamentoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartamentoDto>> Create(DepartamentoCreateDto dto)
    {
        var creado = await _departamentoService.CreateAsync(new Departamento
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion
        });

        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, ToDto(creado));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ProducesResponseType(typeof(DepartamentoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartamentoDto>> Update(int id, DepartamentoUpdateDto dto)
    {
        var actualizado = await _departamentoService.UpdateAsync(id, new Departamento
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion
        });

        return Ok(ToDto(actualizado));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        await _departamentoService.DeleteAsync(id);
        return NoContent();
    }

    private static DepartamentoDto ToDto(Departamento departamento)
    {
        return new DepartamentoDto
        {
            Id = departamento.Id,
            Nombre = departamento.Nombre,
            Descripcion = departamento.Descripcion
        };
    }
}
