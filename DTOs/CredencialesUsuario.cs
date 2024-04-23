using System.ComponentModel.DataAnnotations;

namespace ApiHistorias.DTOs;

public class CredencialesUsuario
{

    public string? Name { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}