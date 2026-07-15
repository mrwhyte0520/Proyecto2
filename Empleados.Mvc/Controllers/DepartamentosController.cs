using Empleados.Data.Entities;
using Empleados.Data.Exceptions;
using Empleados.Data.Services;
using Empleados.Mvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Empleados.Mvc.Controllers;

[Authorize]
public class DepartamentosController : Controller
{
    private readonly IDepartamentoService _departamentoService;

    public DepartamentosController(IDepartamentoService departamentoService)
    {
        _departamentoService = departamentoService;
    }

    public async Task<IActionResult> Index()
    {
        var departamentos = await _departamentoService.GetAllAsync();
        return View(departamentos);
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    public IActionResult Create()
    {
        return View(new DepartamentoFormViewModel());
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartamentoFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        await _departamentoService.CreateAsync(new Departamento { Nombre = vm.Nombre, Descripcion = vm.Descripcion });
        TempData["Mensaje"] = "Departamento creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    public async Task<IActionResult> Edit(int id)
    {
        var departamento = await _departamentoService.GetByIdAsync(id);
        return View(new DepartamentoFormViewModel { Id = departamento.Id, Nombre = departamento.Nombre, Descripcion = departamento.Descripcion });
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.RRHH}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DepartamentoFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        await _departamentoService.UpdateAsync(id, new Departamento { Nombre = vm.Nombre, Descripcion = vm.Descripcion });
        TempData["Mensaje"] = "Departamento actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _departamentoService.DeleteAsync(id);
            TempData["Mensaje"] = "Departamento eliminado correctamente.";
        }
        catch (ConflictException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
