namespace ApiHistorias.DTOs;

public class postPacienteDTO
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? DNI { get; set; }
    public string? OSocial { get; set; }
    public int profesionalId { get; set; }
}