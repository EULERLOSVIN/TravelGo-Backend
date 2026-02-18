// places=darwin
namespace Application.DTOs
{
    public class UpdatePlaceDto
    {
        public int idPlace { get; set; }
        public required string name { get; set; }
        public string? description { get; set; }
    }
}
