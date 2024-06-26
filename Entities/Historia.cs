using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ApiHistorias.Entities;

public class Historia
{
    [Required]
    public int Id { get; set; }
    [Required] 
    public DateTime? Fecha { get; set; } = DateTime.Now;
    [Required]
    [StringLength(maximumLength: 240, ErrorMessage = "Máximo 240 caracteres")]
    public string? Nota { get; set; }
    
    public string? usuarioId { get; set; }
    public IdentityUser? Usuario { get; set; }
    
    //Las dos son 1:1, cada historia se asocia solo a un profesional y a un paciente
    public Profesional? Profesional { get; set; }
    public Paciente? Paciente { get; set; }
}