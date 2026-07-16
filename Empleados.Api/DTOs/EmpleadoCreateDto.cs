using System.ComponentModel.DataAnnotations;

namespace Empleados.Api.DTOs;

public class EmpleadoCreateDto
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un departamento válido.")]
    public int DepartamentoId { get; set; }

    [Required]
    [StringLength(100)]
    public string Puesto { get; set; } = string.Empty;

    [Range(0, 100000000)]
    public decimal Salario { get; set; }

    [Required]
    public DateTime FechaNacimiento { get; set; }

    [Required]
    public DateTime FechaContratacion { get; set; }

    [Required]
    [StringLength(200)]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;
}
