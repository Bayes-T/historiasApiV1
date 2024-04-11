using System.ComponentModel.DataAnnotations;

namespace ApiHistorias.Entities;

public class Profesional
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength: 25, ErrorMessage = "Máximo 25 caracteres")]
    public string? Nombre { get; set; }
    [Required]
    [StringLength(maximumLength: 12, ErrorMessage = "Máximo 12 caracteres")]
    public string? Cargo { get; set; }
    public int? Permisos { get; set; } = 1;
    //profesional:historias 1:m, un profesional puede escribir muchas historias, la relación es de Profesional-Historia
    public List<Historia>? Historias { get; set; }
    //profesional:paciente es 1:m, un profesional puede tener muchos pacientes
    public List<Paciente>? Pacientes { get; set; }
}