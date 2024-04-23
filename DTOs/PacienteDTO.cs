using ApiHistorias.Entities;

namespace ApiHistorias.DTOs;

public class PacienteDTO
{
    //NULLABLE?? ERROR???
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? DNI { get; set; }
    public string? OSocial { get; set; }
    
    public List<Profesional>? Profesionales { get; set; }
    public List<Historia>? Historias { get; set; }
}