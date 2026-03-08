using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Settings
{
    public class ProfileDto
    {
        public int IdAccount { get; set; }
        
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        
        // Opcional para cuando el usuario quiera cambiar su contraseña
        public string? NewPassword { get; set; }
    }
}
