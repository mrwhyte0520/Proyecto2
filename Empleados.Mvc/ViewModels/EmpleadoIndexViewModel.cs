namespace Empleados.Mvc.ViewModels;

public class EmpleadoListItemViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DepartamentoNombre { get; set; } = string.Empty;
    public string Puesto { get; set; } = string.Empty;
    public DateTime FechaContratacion { get; set; }
    public string Estado { get; set; } = string.Empty;
}

public class EmpleadoIndexViewModel
{
    public List<EmpleadoListItemViewModel> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public DateTime? FechaContratacion { get; set; }

    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
