namespace ApiHistorias.DTOs;

public class postHistoriaDTO
{
    public int Id { get; set; }
    public DateTime? Fecha { get; set; }
    public string? Nota { get; set; }
    
    public int pacienteId { get; set; }
    public int? profesionalId { get; set; }
}