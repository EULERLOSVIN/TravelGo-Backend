using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class AddPlaceDto
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression(@".*[a-zA-ZñÑáéíóúÁÉÍÓÚ].*", ErrorMessage = "El nombre debe contener al menos una letra.")]
        public required string name { get; set; }
        public string? description { get; set; }
    }
}
