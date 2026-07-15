namespace Empleados.Mvc.ViewModels;

public class EmpleadoDetailsViewModel
{
    public int Id { get; set; }
    public string? FotografiaRuta { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DepartamentoNombre { get; set; } = string.Empty;
    public string Puesto { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public DateTime FechaContratacion { get; set; }
    public DateTime? FechaTerminacion { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
