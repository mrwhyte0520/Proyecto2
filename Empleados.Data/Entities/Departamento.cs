using System.ComponentModel.DataAnnotations;

namespace Empleados.Data.Entities;

public class Departamento
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Descripcion { get; set; }

    public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
