using System.ComponentModel.DataAnnotations;

namespace Empleados.Api.DTOs;

public class DepartamentoCreateDto
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Descripcion { get; set; }
}
