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
    
    //Tabla auxiliar
    public List<ProfesionalPaciente>? ProfesionalesPacientes { get; set; }
    public List<ProfesionalDTO>? ProfesionalesDTO { get; set; }
}