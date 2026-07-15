using Empleados.Data;
using Microsoft.EntityFrameworkCore;

namespace Empleados.Tests;

public static class TestDbContextFactory
{
    public static EmpleadosDbContext Create()
    {
        var options = new DbContextOptionsBuilder<EmpleadosDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new EmpleadosDbContext(options);
    }
}
