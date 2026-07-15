using Empleados.Data.Entities;
using Empleados.Data.Exceptions;
using Empleados.Data.Services;
using Empleados.Mvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Empleados.Mvc.Controllers;

[Authorize]
public class EmpleadosController : Controller
{
    private const long TamanoMaximoFotoBytes = 5 * 1024 * 1024;
    private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png" };

    private readonly IEmpleadoService _empleadoService;
    private readonly IDepartamentoService _departamentoService;
    private readonly IWebHostEnvironment _environment;

    public EmpleadosController(
        IEmpleadoService empleadoService, IDepartamentoService departamentoService, IWebHostEnvironment environment)
    {
        _empleadoService = empleadoService;
        _departamentoService = departamentoService;
        _environment = environment;
    }

    public async Task<IActionResult> Index(string? nombre, string? apellido, DateTime? fechaContratacion, int page = 1)
    {
        const int pageSize = 10;
        var (items, totalCount) = await _empleadoService.GetPagedAsync(page < 1 ? 1 : page, pageSize, nombre, apellido, fechaContratacion);

        var vm = new EmpleadoIndexViewModel
        {
            Page = page < 1 ? 1 : page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Nombre = nombre,
            Apellido = apellido,
            FechaContratacion = fechaContratacion,
            Items = items.Select(e => new EmpleadoListItemViewModel
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Apellido = e.Apellido,
                DepartamentoNombre = e.Departamento.Nombre,
                Puesto = e.Puesto,
                FechaContratacion = e.FechaContratacion,
                Estado = e.Estado.ToString()
            }).ToList()
        };

        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        var empleado = await _empleadoService.GetByIdAsync(id);
        return View(ToDetailsViewModel(empleado));
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    public async Task<IActionResult> Create()
    {
        var vm = new EmpleadoFormViewModel { DepartamentosDisponibles = await ObtenerDepartamentosSelectListAsync() };
        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmpleadoFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.DepartamentosDisponibles = await ObtenerDepartamentosSelectListAsync();
            return View(vm);
        }

        try
        {
            var creado = await _empleadoService.CreateAsync(new Empleado
            {
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                DepartamentoId = vm.DepartamentoId,
                Puesto = vm.Puesto,
                Salario = vm.Salario,
                FechaNacimiento = vm.FechaNacimiento,
                FechaContratacion = vm.FechaContratacion,
                Direccion = vm.Direccion,
                Telefono = vm.Telefono,
                Email = vm.Email
            });

            TempData["Mensaje"] = "Empleado creado correctamente.";
            return RedirectToAction(nameof(Details), new { id = creado.Id });
        }
        catch (NotFoundException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            vm.DepartamentosDisponibles = await ObtenerDepartamentosSelectListAsync();
            return View(vm);
        }
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    public async Task<IActionResult> Edit(int id)
    {
        var empleado = await _empleadoService.GetByIdAsync(id);
        if (empleado.Estado == EstadoEmpleado.Despedido)
        {
            TempData["Error"] = "Un empleado despedido no puede ser modificado.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var vm = new EmpleadoFormViewModel
        {
            Id = empleado.Id,
            Nombre = empleado.Nombre,
            Apellido = empleado.Apellido,
            DepartamentoId = empleado.DepartamentoId,
            Puesto = empleado.Puesto,
            Salario = empleado.Salario,
            FechaNacimiento = empleado.FechaNacimiento,
            FechaContratacion = empleado.FechaContratacion,
            Direccion = empleado.Direccion,
            Telefono = empleado.Telefono,
            Email = empleado.Email,
            FotografiaRuta = empleado.FotografiaRuta,
            DepartamentosDisponibles = await ObtenerDepartamentosSelectListAsync()
        };

        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmpleadoFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.DepartamentosDisponibles = await ObtenerDepartamentosSelectListAsync();
            return View(vm);
        }

        try
        {
            await _empleadoService.UpdateAsync(id, new Empleado
            {
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                DepartamentoId = vm.DepartamentoId,
                Puesto = vm.Puesto,
                Salario = vm.Salario,
                FechaNacimiento = vm.FechaNacimiento,
                FechaContratacion = vm.FechaContratacion,
                Direccion = vm.Direccion,
                Telefono = vm.Telefono,
                Email = vm.Email
            });

            TempData["Mensaje"] = "Empleado actualizado correctamente.";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex) when (ex is NotFoundException or ConflictException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            vm.DepartamentosDisponibles = await ObtenerDepartamentosSelectListAsync();
            return View(vm);
        }
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubirFoto(int id, IFormFile foto)
    {
        if (foto is null || foto.Length == 0)
        {
            TempData["Error"] = "Debe seleccionar un archivo.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        if (foto.Length > TamanoMaximoFotoBytes)
        {
            TempData["Error"] = "El archivo no puede superar los 5 MB.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var extension = Path.GetExtension(foto.FileName).ToLowerInvariant();
        if (!ExtensionesPermitidas.Contains(extension))
        {
            TempData["Error"] = "Solo se permiten imágenes .jpg, .jpeg o .png.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var carpetaDestino = Path.Combine(_environment.WebRootPath, "uploads", "empleados");
        Directory.CreateDirectory(carpetaDestino);

        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);

        await using (var stream = new FileStream(rutaFisica, FileMode.Create))
        {
            await foto.CopyToAsync(stream);
        }

        await _empleadoService.SetFotografiaAsync(id, $"/uploads/empleados/{nombreArchivo}");

        TempData["Mensaje"] = "Fotografía actualizada correctamente.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Desactivar(int id)
    {
        await _empleadoService.DesactivarAsync(id);
        TempData["Mensaje"] = "Empleado desactivado.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reactivar(int id)
    {
        await _empleadoService.ReactivarAsync(id);
        TempData["Mensaje"] = "Empleado reactivado.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Despedir(int id)
    {
        await _empleadoService.DespedirAsync(id, null);
        TempData["Mensaje"] = "Empleado despedido.";
        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task<IEnumerable<SelectListItem>> ObtenerDepartamentosSelectListAsync()
    {
        var departamentos = await _departamentoService.GetAllAsync();
        return departamentos.Select(d => new SelectListItem(d.Nombre, d.Id.ToString()));
    }

    private static EmpleadoDetailsViewModel ToDetailsViewModel(Empleado empleado)
    {
        return new EmpleadoDetailsViewModel
        {
            Id = empleado.Id,
            FotografiaRuta = empleado.FotografiaRuta,
            Nombre = empleado.Nombre,
            Apellido = empleado.Apellido,
            DepartamentoNombre = empleado.Departamento.Nombre,
            Puesto = empleado.Puesto,
            Salario = empleado.Salario,
            FechaNacimiento = empleado.FechaNacimiento,
            FechaContratacion = empleado.FechaContratacion,
            FechaTerminacion = empleado.FechaTerminacion,
            Direccion = empleado.Direccion,
            Telefono = empleado.Telefono,
            Email = empleado.Email,
            Estado = empleado.Estado.ToString()
        };
    }
}
