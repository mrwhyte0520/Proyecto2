using Empleados.Data.Entities;
using Empleados.Data.Exceptions;
using Empleados.Data.Services;
using Xunit;

namespace Empleados.Tests.Services;

public class DepartamentoServiceTests
{
    [Fact]
    public async Task DeleteAsync_DepartamentoConEmpleadosAsociados_LanzaConflictException()
    {
        var context = TestDbContextFactory.Create();
        var departamento = new Departamento { Nombre = "Ventas" };
        context.Departamentos.Add(departamento);
        await context.SaveChangesAsync();

        context.Empleados.Add(new Empleado
        {
            Nombre = "Luis",
            Apellido = "Gómez",
            DepartamentoId = departamento.Id,
            Puesto = "Vendedor",
            Salario = 30000,
            FechaNacimiento = new DateTime(1990, 1, 1),
            FechaContratacion = new DateTime(2022, 1, 1),
            Direccion = "Calle 2",
            Telefono = "8095550001",
            Email = "luis.gomez@empresa.com"
        });
        await context.SaveChangesAsync();

        var service = new DepartamentoService(context);

        await Assert.ThrowsAsync<ConflictException>(() => service.DeleteAsync(departamento.Id));
    }

    [Fact]
    public async Task DeleteAsync_DepartamentoSinEmpleados_LoElimina()
    {
        var context = TestDbContextFactory.Create();
        var departamento = new Departamento { Nombre = "Marketing" };
        context.Departamentos.Add(departamento);
        await context.SaveChangesAsync();

        var service = new DepartamentoService(context);
        await service.DeleteAsync(departamento.Id);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(departamento.Id));
    }
}
