using ApiHistorias.Entities;

namespace ApiHistorias.DTOs;

public class ProfesionalDTO
{
    public int? Id { get; set; }
    public string? Nombre { get; set; }
    public string? Cargo { get; set; }
    public int? Permisos { get; set; } = 1;
    //profesional:historias 1:m, un profesional puede escribir muchas historias, la relación es de Profesional-Historia
    public List<Historia>? Historias { get; set; }
    //profesional:paciente es 1:m, un profesional puede tener muchos pacientes
    public List<Paciente>? Pacientes { get; set; }
    
    //Tabla auxiliar
    public List<ProfesionalPaciente>? ProfesionalesPacientes { get; set; }
}