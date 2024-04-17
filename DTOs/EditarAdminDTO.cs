using System.ComponentModel.DataAnnotations;

namespace ApiHistorias.DTOs;

public class EditarAdminDTO
{
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    
}