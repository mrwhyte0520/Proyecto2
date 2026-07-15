using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Empleados.Data.Entities;

public class Empleado
{
    public int Id { get; set; }

    [StringLength(260)]
    public string? FotografiaRuta { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;

    public int DepartamentoId { get; set; }
    public Departamento Departamento { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Puesto { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Salario { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaNacimiento { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaContratacion { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaTerminacion { get; set; }

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
    [RegularExpression(@"^[^@\s]+@gmail\.com$", ErrorMessage = "El correo debe ser una cuenta de Gmail (@gmail.com).")]
    public string Email { get; set; } = string.Empty;

    public EstadoEmpleado Estado { get; set; } = EstadoEmpleado.Activo;
}
