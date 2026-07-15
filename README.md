# Sistema de Gestión de Empleados

Sistema para gestionar empleados y departamentos, compuesto por una API REST y una aplicación web MVC que comparten la misma base de datos. Desarrollado en **.NET 8** con **Entity Framework Core** y **ASP.NET Core Identity**.

## Estructura del proyecto

| Proyecto | Descripción |
|---|---|
| `Empleados.Data` | Núcleo compartido: `DbContext`, entidades, servicios de dominio y seed de datos. |
| `Empleados.Api` | API REST con autenticación JWT y documentación Swagger. |
| `Empleados.Mvc` | Aplicación web con vistas Razor y autenticación por cookies (Identity). |
| `Empleados.Tests` | Pruebas unitarias (xUnit + EF Core InMemory). |

`Empleados.Api` y `Empleados.Mvc` son independientes entre sí: ambos se conectan directo a la base de datos mediante `EmpleadosDbContext`, ninguno llama al otro por HTTP.

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local o remoto)
- Herramienta `dotnet-ef` para migraciones (opcional):
  ```
  dotnet tool install --global dotnet-ef
  ```

## Configuración

1. Ajusta la cadena de conexión en `appsettings.Development.json` de **ambos** proyectos (`Empleados.Api` y `Empleados.Mvc`):

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=TU_SERVIDOR;Database=Practica2;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
   }
   ```

2. Aplica las migraciones (desde `Empleados.Data`, indicando el proyecto de inicio):

   ```
   dotnet ef database update --startup-project ../Empleados.Api
   ```

   Al iniciar la aplicación, el seed automático crea los roles (`Admin`, `RRHH`) y dos usuarios de acceso:

   | Correo | Contraseña | Rol |
   |---|---|---|
   | `admin@gmail.com` | `Admin123!` | Admin |
   | `rrhh@gmail.com` | `Rrhh123!` | RRHH |

## Cómo ejecutar

Cada proyecto corre de forma independiente:

```
dotnet run --project Empleados.Api
dotnet run --project Empleados.Mvc
```

| Proyecto | URL (http) | URL (https) |
|---|---|---|
| Empleados.Api | http://localhost:5119/swagger | https://localhost:7076/swagger |
| Empleados.Mvc | http://localhost:5233 | https://localhost:7170 |

Para levantar ambos a la vez desde Visual Studio: clic derecho en la solución → **Set Startup Projects** → **Multiple startup projects** → poner ambos en *Start*.

## Funcionalidad

- **Empleados**: listado paginado con filtros (nombre, apellido, fecha de contratación), detalle, alta, edición, foto de perfil, desactivar/reactivar y despedir.
- **Departamentos**: listado, alta, edición y eliminación.
- **Roles**: `Admin` tiene acceso total; `RRHH` puede gestionar empleados y departamentos (no puede eliminar departamentos); usuarios sin rol solo pueden consultar.
- **Validación de correo**: tanto el correo de los empleados como el de los usuarios del sistema deben ser cuentas `@gmail.com`.

## Pruebas

```
dotnet test
```

## Notas

- `appsettings.Development.json` está excluido del control de versiones (`.gitignore`) porque contiene la cadena de conexión y la clave JWT — cada entorno debe crear el suyo a partir de su propia configuración local.
- Las fotos de empleados subidas se guardan en `Empleados.Mvc/Empleados.Api` bajo `wwwroot/uploads/`, también excluido del control de versiones.
