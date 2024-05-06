namespace ApiHistorias.Entities;

public class ProfesionalPaciente
{
    public int ProfesionalId { get; set; }
    public int PacienteId { get; set; }
    
    //nav props
    public Profesional Profesional { get; set; }
    public Paciente Paciente { get; set; }
}