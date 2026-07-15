using Empleados.Data.Entities;

namespace Empleados.Data.Services;

public interface IEmpleadoService
{
    Task<(List<Empleado> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? nombre, string? apellido, DateTime? fechaContratacion);

    Task<Empleado> GetByIdAsync(int id);

    Task<Empleado> CreateAsync(Empleado empleado);

    Task<Empleado> UpdateAsync(int id, Empleado datosActualizados);

    Task DesactivarAsync(int id);

    Task ReactivarAsync(int id);

    Task DespedirAsync(int id, DateTime? fechaTerminacion);

    Task<Empleado> SetFotografiaAsync(int id, string rutaRelativa);
}
