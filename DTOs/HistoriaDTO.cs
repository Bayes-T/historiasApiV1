using ApiHistorias.Entities;

namespace ApiHistorias.DTOs;

public class HistoriaDTO
{
    public int? Id { get; set; }
    public DateTime? Fecha { get; set; }
    public string? Nota { get; set; }
    
    public Paciente? Paciente { get; set; }
    public Profesional? Profesional { get; set; }
}