using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ApiHistorias.Entities;

public class Paciente
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength: 25, ErrorMessage = "Máximo 25 caracteres")]
    public string? Nombre { get; set; }
    [Required]
    [StringLength(maximumLength: 8, ErrorMessage = "Máximo 8 caracteres")]
    public string? DNI { get; set; }
    [Required]
    [StringLength(maximumLength: 25, ErrorMessage = "Máximo 25 caracteres")]
    public string? OSocial { get; set; }
    
    //paciente:profesional 1:1, un paciente puede tener un profesional al tiempo, aunque pueda tener varios profesionales a lo largo del tiempo
    public List<Profesional>? Profesionales { get; set; }
    //paciente:historia 1:m, un paciente puede tener muchas historias, la relación es de paciente-historia
    public List<Historia>? Historias { get; set; }
    
    //Tabla auxiliar
    public List<ProfesionalPaciente>? ProfesionalesPacientes { get; set; }
}