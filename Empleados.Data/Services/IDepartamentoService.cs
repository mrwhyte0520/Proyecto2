using Empleados.Data.Entities;

namespace Empleados.Data.Services;

public interface IDepartamentoService
{
    Task<List<Departamento>> GetAllAsync();

    Task<Departamento> GetByIdAsync(int id);

    Task<Departamento> CreateAsync(Departamento departamento);

    Task<Departamento> UpdateAsync(int id, Departamento datosActualizados);

    Task DeleteAsync(int id);
}
