using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Empleados.Mvc.ViewModels;

public class EmpleadoFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un departamento.")]
    [Display(Name = "Departamento")]
    public int DepartamentoId { get; set; }

    [Required(ErrorMessage = "El puesto es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Puesto")]
    public string Puesto { get; set; } = string.Empty;

    [Range(0, 100000000, ErrorMessage = "El salario debe ser un valor positivo.")]
    [Display(Name = "Salario")]
    public decimal Salario { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de nacimiento")]
    public DateTime FechaNacimiento { get; set; } = DateTime.Today.AddYears(-25);

    [Required(ErrorMessage = "La fecha de contratación es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de contratación")]
    public DateTime FechaContratacion { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    [StringLength(200)]
    [Display(Name = "Dirección")]
    public string Direccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Ingrese un teléfono válido.")]
    [StringLength(20)]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
    [StringLength(150)]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Fotografía")]
    public IFormFile? Foto { get; set; }

    public string? FotografiaRuta { get; set; }

    public IEnumerable<SelectListItem> DepartamentosDisponibles { get; set; } = Enumerable.Empty<SelectListItem>();
}
