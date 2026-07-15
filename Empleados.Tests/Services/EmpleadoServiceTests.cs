using Empleados.Data.Entities;
using Empleados.Data.Exceptions;
using Empleados.Data.Services;
using Xunit;

namespace Empleados.Tests.Services;

public class EmpleadoServiceTests
{
    private static async Task<(EmpleadoService Service, Empleado Empleado)> CrearEmpleadoDePruebaAsync()
    {
        var context = TestDbContextFactory.Create();
        var departamento = new Departamento { Nombre = "Tecnología" };
        context.Departamentos.Add(departamento);
        await context.SaveChangesAsync();

        var service = new EmpleadoService(context);
        var empleado = await service.CreateAsync(new Empleado
        {
            Nombre = "Ana",
            Apellido = "Pérez",
            DepartamentoId = departamento.Id,
            Puesto = "Desarrolladora",
            Salario = 40000,
            FechaNacimiento = new DateTime(1995, 1, 1),
            FechaContratacion = new DateTime(2023, 1, 1),
            Direccion = "Calle 1",
            Telefono = "8095550000",
            Email = "ana.perez@empresa.com"
        });

        return (service, empleado);
    }

    [Fact]
    public async Task DesactivarAsync_CambiaEstadoAInactivo()
    {
        var (service, empleado) = await CrearEmpleadoDePruebaAsync();

        await service.DesactivarAsync(empleado.Id);

        var actualizado = await service.GetByIdAsync(empleado.Id);
        Assert.Equal(EstadoEmpleado.Inactivo, actualizado.Estado);
    }

    [Fact]
    public async Task DespedirAsync_EstableceEstadoDespedidoYFechaTerminacion()
    {
        var (service, empleado) = await CrearEmpleadoDePruebaAsync();
        var fecha = new DateTime(2026, 1, 15);

        await service.DespedirAsync(empleado.Id, fecha);

        var actualizado = await service.GetByIdAsync(empleado.Id);
        Assert.Equal(EstadoEmpleado.Despedido, actualizado.Estado);
        Assert.Equal(fecha, actualizado.FechaTerminacion);
    }

    [Fact]
    public async Task DespedirAsync_EsIrreversible_NoPermiteActualizarDespues()
    {
        var (service, empleado) = await CrearEmpleadoDePruebaAsync();
        await service.DespedirAsync(empleado.Id, null);

        var datosActualizados = new Empleado
        {
            Nombre = "Ana",
            Apellido = "Pérez",
            DepartamentoId = empleado.DepartamentoId,
            Puesto = "Otro puesto",
            Salario = 50000,
            FechaNacimiento = empleado.FechaNacimiento,
            FechaContratacion = empleado.FechaContratacion,
            Direccion = empleado.Direccion,
            Telefono = empleado.Telefono,
            Email = empleado.Email
        };

        await Assert.ThrowsAsync<ConflictException>(() => service.UpdateAsync(empleado.Id, datosActualizados));
    }

    [Fact]
    public async Task GetByIdAsync_EmpleadoNoExiste_LanzaNotFoundException()
    {
        var (service, _) = await CrearEmpleadoDePruebaAsync();

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(9999));
    }
}
