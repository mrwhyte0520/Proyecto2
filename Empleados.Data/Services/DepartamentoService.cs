using Empleados.Data.Entities;
using Empleados.Data.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Empleados.Data.Services;

public class DepartamentoService : IDepartamentoService
{
    private readonly EmpleadosDbContext _context;

    public DepartamentoService(EmpleadosDbContext context)
    {
        _context = context;
    }

    public Task<List<Departamento>> GetAllAsync()
    {
        return _context.Departamentos.OrderBy(d => d.Nombre).ToListAsync();
    }

    public async Task<Departamento> GetByIdAsync(int id)
    {
        var departamento = await _context.Departamentos.FirstOrDefaultAsync(d => d.Id == id);
        return departamento ?? throw new NotFoundException($"No se encontró el departamento con id {id}.");
    }

    public async Task<Departamento> CreateAsync(Departamento departamento)
    {
        _context.Departamentos.Add(departamento);
        await _context.SaveChangesAsync();
        return departamento;
    }

    public async Task<Departamento> UpdateAsync(int id, Departamento datosActualizados)
    {
        var departamento = await GetByIdAsync(id);
        departamento.Nombre = datosActualizados.Nombre;
        departamento.Descripcion = datosActualizados.Descripcion;
        await _context.SaveChangesAsync();
        return departamento;
    }

    public async Task DeleteAsync(int id)
    {
        var departamento = await GetByIdAsync(id);

        var tieneEmpleados = await _context.Empleados.AnyAsync(e => e.DepartamentoId == id);
        if (tieneEmpleados)
        {
            throw new ConflictException("No se puede eliminar un departamento que tiene empleados asociados.");
        }

        _context.Departamentos.Remove(departamento);
        await _context.SaveChangesAsync();
    }
}
