namespace ApiHistorias.DTOs;

public class postProfesionalDTO
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Cargo { get; set; }
    public int? Permisos { get; set; } = 1;
    
    public List<int>? PacientesId { get; set; }
}