using Empleados.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Empleados.Data;

public class EmpleadosDbContext : IdentityDbContext<ApplicationUser>
{
    public EmpleadosDbContext(DbContextOptions<EmpleadosDbContext> options) : base(options)
    {
    }

    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Empleado> Empleados => Set<Empleado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasIndex(d => d.Nombre).IsUnique();

            entity.HasMany(d => d.Empleados)
                  .WithOne(e => e.Departamento)
                  .HasForeignKey(e => e.DepartamentoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.Property(e => e.Estado)
                  .HasConversion<string>()
                  .HasMaxLength(20);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departamento>().HasData(
            new Departamento { Id = 1, Nombre = "Recursos Humanos", Descripcion = "Gestión del talento humano" },
            new Departamento { Id = 2, Nombre = "Tecnología", Descripcion = "Desarrollo y soporte de sistemas" },
            new Departamento { Id = 3, Nombre = "Ventas", Descripcion = "Comercialización y atención a clientes" }
        );

        modelBuilder.Entity<Empleado>().HasData(
            new Empleado
            {
                Id = 1, Nombre = "Juan", Apellido = "Pérez", DepartamentoId = 2, Puesto = "Desarrollador Backend",
                Salario = 45000m, FechaNacimiento = new DateTime(1992, 3, 14), FechaContratacion = new DateTime(2020, 1, 15),
                Direccion = "Calle 10 #23-45", Telefono = "8095551001", Email = "juan.perez@gmail.com", Estado = EstadoEmpleado.Activo
            },
            new Empleado
            {
                Id = 2, Nombre = "María", Apellido = "Gómez", DepartamentoId = 2, Puesto = "Desarrolladora Frontend",
                Salario = 42000m, FechaNacimiento = new DateTime(1994, 7, 22), FechaContratacion = new DateTime(2021, 3, 1),
                Direccion = "Av. Independencia #12", Telefono = "8095551002", Email = "maria.gomez@gmail.com", Estado = EstadoEmpleado.Activo
            },
            new Empleado
            {
                Id = 3, Nombre = "Carlos", Apellido = "Rodríguez", DepartamentoId = 1, Puesto = "Analista de RRHH",
                Salario = 38000m, FechaNacimiento = new DateTime(1988, 11, 5), FechaContratacion = new DateTime(2018, 6, 10),
                Direccion = "Calle Duarte #78", Telefono = "8095551003", Email = "carlos.rodriguez@gmail.com", Estado = EstadoEmpleado.Activo
            },
            new Empleado
            {
                Id = 4, Nombre = "Ana", Apellido = "Martínez", DepartamentoId = 1, Puesto = "Gerente de RRHH",
                Salario = 60000m, FechaNacimiento = new DateTime(1985, 2, 18), FechaContratacion = new DateTime(2015, 9, 1),
                Direccion = "Av. 27 de Febrero #200", Telefono = "8095551004", Email = "ana.martinez@gmail.com", Estado = EstadoEmpleado.Activo
            },
            new Empleado
            {
                Id = 5, Nombre = "Luis", Apellido = "Fernández", DepartamentoId = 3, Puesto = "Ejecutivo de Ventas",
                Salario = 35000m, FechaNacimiento = new DateTime(1996, 5, 30), FechaContratacion = new DateTime(2022, 2, 14),
                Direccion = "Calle El Sol #56", Telefono = "8095551005", Email = "luis.fernandez@gmail.com", Estado = EstadoEmpleado.Activo
            },
            new Empleado
            {
                Id = 6, Nombre = "Sofía", Apellido = "Castillo", DepartamentoId = 3, Puesto = "Gerente de Ventas",
                Salario = 55000m, FechaNacimiento = new DateTime(1987, 9, 9), FechaContratacion = new DateTime(2016, 4, 20),
                Direccion = "Av. Winston Churchill #300", Telefono = "8095551006", Email = "sofia.castillo@gmail.com", Estado = EstadoEmpleado.Activo
            },
            new Empleado
            {
                Id = 7, Nombre = "Pedro", Apellido = "Ramírez", DepartamentoId = 2, Puesto = "Administrador de Bases de Datos",
                Salario = 48000m, FechaNacimiento = new DateTime(1990, 12, 1), FechaContratacion = new DateTime(2019, 8, 5),
                Direccion = "Calle Mercedes #89", Telefono = "8095551007", Email = "pedro.ramirez@gmail.com", Estado = EstadoEmpleado.Inactivo
            },
            new Empleado
            {
                Id = 8, Nombre = "Laura", Apellido = "Jiménez", DepartamentoId = 1, Puesto = "Reclutadora",
                Salario = 33000m, FechaNacimiento = new DateTime(1995, 4, 25), FechaContratacion = new DateTime(2021, 11, 8),
                Direccion = "Calle Palma #34", Telefono = "8095551008", Email = "laura.jimenez@gmail.com", Estado = EstadoEmpleado.Activo
            },
            new Empleado
            {
                Id = 9, Nombre = "Miguel", Apellido = "Torres", DepartamentoId = 3, Puesto = "Ejecutivo de Ventas",
                Salario = 34000m, FechaNacimiento = new DateTime(1993, 6, 17), FechaContratacion = new DateTime(2020, 7, 1),
                FechaTerminacion = new DateTime(2023, 12, 15),
                Direccion = "Av. Máximo Gómez #150", Telefono = "8095551009", Email = "miguel.torres@gmail.com", Estado = EstadoEmpleado.Despedido
            },
            new Empleado
            {
                Id = 10, Nombre = "Daniela", Apellido = "Vargas", DepartamentoId = 2, Puesto = "Tester QA",
                Salario = 39000m, FechaNacimiento = new DateTime(1997, 8, 3), FechaContratacion = new DateTime(2022, 5, 16),
                Direccion = "Calle Restauración #67", Telefono = "8095551010", Email = "daniela.vargas@gmail.com", Estado = EstadoEmpleado.Activo
            }
        );
    }
}
