using Empleados.Data.Entities;
using Empleados.Data.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Empleados.Data.Services;

public class EmpleadoService : IEmpleadoService
{
    private readonly EmpleadosDbContext _context;

    public EmpleadoService(EmpleadosDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Empleado> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? nombre, string? apellido, DateTime? fechaContratacion)
    {
        var query = _context.Empleados.Include(e => e.Departamento).AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            query = query.Where(e => e.Nombre.Contains(nombre));
        }

        if (!string.IsNullOrWhiteSpace(apellido))
        {
            query = query.Where(e => e.Apellido.Contains(apellido));
        }

        if (fechaContratacion.HasValue)
        {
            query = query.Where(e => e.FechaContratacion.Date == fechaContratacion.Value.Date);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(e => e.Apellido)
            .ThenBy(e => e.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Empleado> GetByIdAsync(int id)
    {
        var empleado = await _context.Empleados
            .Include(e => e.Departamento)
            .FirstOrDefaultAsync(e => e.Id == id);

        return empleado ?? throw new NotFoundException($"No se encontró el empleado con id {id}.");
    }

    public async Task<Empleado> CreateAsync(Empleado empleado)
    {
        await ValidarDepartamentoExisteAsync(empleado.DepartamentoId);

        empleado.Estado = EstadoEmpleado.Activo;
        _context.Empleados.Add(empleado);
        await _context.SaveChangesAsync();
        return empleado;
    }

    public async Task<Empleado> UpdateAsync(int id, Empleado datosActualizados)
    {
        var empleado = await GetByIdAsync(id);
        AsegurarNoDespedido(empleado);
        await ValidarDepartamentoExisteAsync(datosActualizados.DepartamentoId);

        empleado.Nombre = datosActualizados.Nombre;
        empleado.Apellido = datosActualizados.Apellido;
        empleado.DepartamentoId = datosActualizados.DepartamentoId;
        empleado.Puesto = datosActualizados.Puesto;
        empleado.Salario = datosActualizados.Salario;
        empleado.FechaNacimiento = datosActualizados.FechaNacimiento;
        empleado.FechaContratacion = datosActualizados.FechaContratacion;
        empleado.Direccion = datosActualizados.Direccion;
        empleado.Telefono = datosActualizados.Telefono;
        empleado.Email = datosActualizados.Email;

        await _context.SaveChangesAsync();
        return empleado;
    }

    public async Task DesactivarAsync(int id)
    {
        var empleado = await GetByIdAsync(id);
        AsegurarNoDespedido(empleado);

        empleado.Estado = EstadoEmpleado.Inactivo;
        await _context.SaveChangesAsync();
    }

    public async Task ReactivarAsync(int id)
    {
        var empleado = await GetByIdAsync(id);
        AsegurarNoDespedido(empleado);

        empleado.Estado = EstadoEmpleado.Activo;
        await _context.SaveChangesAsync();
    }

    public async Task DespedirAsync(int id, DateTime? fechaTerminacion)
    {
        var empleado = await GetByIdAsync(id);
        AsegurarNoDespedido(empleado);

        empleado.Estado = EstadoEmpleado.Despedido;
        empleado.FechaTerminacion = fechaTerminacion?.Date ?? DateTime.UtcNow.Date;
        await _context.SaveChangesAsync();
    }

    public async Task<Empleado> SetFotografiaAsync(int id, string rutaRelativa)
    {
        var empleado = await GetByIdAsync(id);
        AsegurarNoDespedido(empleado);

        empleado.FotografiaRuta = rutaRelativa;
        await _context.SaveChangesAsync();
        return empleado;
    }

    private static void AsegurarNoDespedido(Empleado empleado)
    {
        if (empleado.Estado == EstadoEmpleado.Despedido)
        {
            throw new ConflictException("Un empleado despedido no puede ser modificado, solo consultado.");
        }
    }

    private async Task ValidarDepartamentoExisteAsync(int departamentoId)
    {
        var existe = await _context.Departamentos.AnyAsync(d => d.Id == departamentoId);
        if (!existe)
        {
            throw new NotFoundException($"No se encontró el departamento con id {departamentoId}.");
        }
    }
}
